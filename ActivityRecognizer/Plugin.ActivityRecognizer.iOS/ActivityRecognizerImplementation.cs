using CoreMotion;
using Foundation;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Plugin.ActivityRecognizer
{
    /// <summary>
    /// Implementation for ActivityRecognizer
    /// </summary>
    public class ActivityRecognizerImplementation : IActivityRecognizer
    {
        private readonly CMMotionActivityManager MotionActivityManager;

        public bool IsSupported => CMMotionActivityManager.IsActivityAvailable;

        public ActivityRecognizerImplementation()
        {
            this.MotionActivityManager = new CMMotionActivityManager();

            InitMotionDetectedObservable();
        }

        private IConnectableObservable<DetectedMotionResult> MotionDetectedObservable;
        private void InitMotionDetectedObservable()
        {
            this.MotionDetectedObservable = Observable.Create<DetectedMotionResult>(ob =>
            {
                this.MotionActivityManager.StartActivityUpdates(NSOperationQueue.MainQueue, activity =>
                {
                    var dateTime = activity.StartDate.ToDateTime();

                    var mostProbable = new DetectedMotion(activity.Confidence.ToPercentage(), activity.GetMotionType());
                    var detectedMotions = CreatePossibleMotions(activity).ToArray();

                    ob.OnNext(new DetectedMotionResult(dateTime, mostProbable, detectedMotions));
                });

                return () =>
                {
                    this.MotionActivityManager.StopActivityUpdates();
                    this.MotionActivityManager.Dispose();
                };
            })
            .Replay(1);

            this.MotionDetectedObservable.Connect();
        }

        private static List<DetectedMotion> CreatePossibleMotions(CMMotionActivity activity)
        {
            List<DetectedMotion> result = new List<DetectedMotion>();
            int confidence = activity.Confidence.ToPercentage();

            if (activity.Automotive)
                result.Add(new DetectedMotion(confidence, MotionType.InVehicle));
            else if (activity.Cycling)
                result.Add(new DetectedMotion(confidence, MotionType.OnBicycle));
            else if (activity.Running)
                result.Add(new DetectedMotion(confidence, MotionType.Running));
            else if (activity.Walking)
                result.Add(new DetectedMotion(confidence, MotionType.Walking));
            else if (activity.Stationary)
                result.Add(new DetectedMotion(confidence, MotionType.Stationary));
            else
                result.Add(new DetectedMotion(confidence, MotionType.Unknown));

            return result;
        }

        public IObservable<DetectedMotionResult> WhenMotionDetected()
        {
            return this.MotionDetectedObservable;
        }

        public IObservable<DetectedMotionResult> GetLastMotion()
        {
            return this.MotionDetectedObservable
                       .Take(1);
        }
    }
}