using Android.Gms.Location;
using System;

namespace Plugin.ActivityRecognizer
{
    internal sealed class DetectedActivitiesEventArgs : EventArgs
    {
        public DetectedActivity MostProbable { get; }
        public DetectedActivity SecondMostProbable { get; }

        public DetectedActivitiesEventArgs(DetectedActivity[] activities)
        {
            this.MostProbable = activities[0];
            this.SecondMostProbable = activities[1];
        }
    }
}