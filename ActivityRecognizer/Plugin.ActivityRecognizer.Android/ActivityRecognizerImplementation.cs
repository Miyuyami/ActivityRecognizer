using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Support.V4.Content;
using MiscUtils.Logging;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Plugin.ActivityRecognizer
{
    /// <summary>
    /// Implementation for ActivityRecognizer
    /// </summary>
    public class ActivityRecognizerImplementation : IActivityRecognizer
    {
        private readonly GoogleApiClient _Client;

        public bool IsSupported => GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context) == ConnectionResult.Success;

        public Status Status { get; } = Status.Disconnected;

        public ActivityRecognizerImplementation()
        {
            this._Client = new GoogleApiClient.Builder(Application.Context)
                                              .AddConnectionCallbacks(this.OnConnected, this.OnConnectionSuspended)
                                              .AddOnConnectionFailedListener(this.OnConnectionFailed)
                                              .AddApi(ActivityRecognition.API)
                                              .Build();

            InitMotionDetectedObservable();
        }

        #region GoogleApiClient callbacks and Status observable
        private Subject<Status> StatusChangedObservable = new Subject<Status>();

        private void OnConnected(Bundle connectionHint)
        {
            DebugLogger.Write($"Connected: {connectionHint}.");
            this.StatusChangedObservable.OnNext(Status.Connected);
        }

        private void OnConnectionSuspended(int cause)
        {
            DebugLogger.Write($"Connection suspended: {cause}.");
            if (GoogleApiClient.ConnectionCallbacks.CauseServiceDisconnected == cause)
            {
                this.StatusChangedObservable.OnNext(Status.Disconnected);
            }
            else if (GoogleApiClient.ConnectionCallbacks.CauseNetworkLost == cause)
            {
                this.StatusChangedObservable.OnNext(Status.NetworkLost);
            }
        }

        private void OnConnectionFailed(ConnectionResult result)
        {
            DebugLogger.Write($"Connection failed: {result}.");
            this.StatusChangedObservable.OnNext(Status.Error);
            this._Client.Connect();
            //this.StatusChangedObservable.OnError(new Exception(result.ErrorMessage));
        }

        public IObservable<Status> WhenStatusChanged()
        {
            return this.StatusChangedObservable
                       .StartWith(this.Status)
                       .Publish()
                       .RefCount();
        }
        #endregion

        private IConnectableObservable<DetectedMotionResult> MotionDetectedObservable;
        private void InitMotionDetectedObservable()
        {
            this.MotionDetectedObservable = Observable.Create<DetectedMotionResult>(async ob =>
            {
                this._Client.Connect(GoogleApiClient.SignInModeOptional);
                await this.WhenStatusChanged().Any(s => s == Status.Connected);

                var broadcastReceiver = new EventBroadcastReceiver();
                LocalBroadcastManager.GetInstance(Application.Context).RegisterReceiver(broadcastReceiver, new IntentFilter(Constants.BroadcastAction));
                var sub1 = Observable.FromEventPattern<BroadcastEventArgs>(h => broadcastReceiver.Received += h, h => broadcastReceiver.Received -= h)
                                     .Subscribe(e =>
                                     {
                                         var activityResult = (ActivityRecognitionResult)e.EventArgs.Intent.GetParcelableExtra(Constants.ActivitiesExtra);

                                         DetectedMotion mostProbableMotion = GetMostProbableMotion(activityResult);
                                         DetectedMotion[] motions = activityResult.ProbableActivities.Select(da => new DetectedMotion(da.Confidence, da.GetMotionType()))
                                                                                                     .ToArray();

                                         ob.OnNext(new DetectedMotionResult(new DateTime(activityResult.Time, DateTimeKind.Utc), mostProbableMotion, motions));
                                     });

                var intent = new Intent(Application.Context, typeof(DetectedActivitiesIntentService));
                var pendingIntent = PendingIntent.GetService(Application.Context, 0, intent, PendingIntentFlags.UpdateCurrent);
                Statuses result = await ActivityRecognition.ActivityRecognitionApi.RequestActivityUpdatesAsync(this._Client, 0L, pendingIntent); // TODO: instead of 0L, use Config property, make sure the Config object is captured inside the lambda.
                if (!result.IsSuccess)
                {
                    ob.OnError(new Exception("Request failed"));
                }

                return () =>
                {
                    sub1.Dispose();
                    LocalBroadcastManager.GetInstance(Application.Context).UnregisterReceiver(broadcastReceiver);
                    broadcastReceiver.Dispose();
                    this._Client.Disconnect();
                    this._Client.Dispose();
                };
            })
            .Replay(1);

            this.MotionDetectedObservable.Connect(); // TODO? store this somewhere?
        }

        private static DetectedMotion GetMostProbableMotion(ActivityRecognitionResult activityResult)
        {
            const int threshold = 5;
            DetectedActivity first = activityResult.MostProbableActivity;
            int type = first.Type;

            // OnFoot activity is sent for all motion activities, it's redundant so we remove it...
            if (type == DetectedActivity.OnFoot)
            {
                DetectedActivity second = activityResult.ProbableActivities.Where(da => da.Type != DetectedActivity.OnFoot)
                                                                           .DefaultIfEmpty()
                                                                           .Aggregate((max, x) => max.Confidence < x.Confidence ? x : max);

                if (second != null && second.Confidence >= first.Confidence - threshold)
                {
                    return new DetectedMotion(second.Confidence, second.GetMotionType());
                }
            }

            return new DetectedMotion(first.Confidence, first.GetMotionType());
        }

        public IObservable<DetectedMotionResult> WhenMotionDetected()
        {
            return this.MotionDetectedObservable;
        }

        public IObservable<DetectedMotionResult> GetMotion()
        {
            return this.MotionDetectedObservable
                       .Take(1);
        }
    }
}