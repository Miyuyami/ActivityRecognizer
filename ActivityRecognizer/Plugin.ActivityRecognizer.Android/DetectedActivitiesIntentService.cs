using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Support.V4.Content;

namespace Plugin.ActivityRecognizer
{
    [Service(Exported = false)]
    internal class DetectedActivitiesIntentService : IntentService
    {
        private const string Name = "detected-activities-intent-service";

        public DetectedActivitiesIntentService() : base(Name)
        {

        }

        protected override void OnHandleIntent(Intent intent)
        {
            var result = ActivityRecognitionResult.ExtractResult(intent);
            if (result == null)
            {
                return;
            }

            var broadcastIntent = new Intent(Constants.BroadcastAction);
            broadcastIntent.PutExtra(Constants.ActivitiesExtra, result);
            LocalBroadcastManager.GetInstance(Application.Context).SendBroadcast(broadcastIntent);
        }
    }
}