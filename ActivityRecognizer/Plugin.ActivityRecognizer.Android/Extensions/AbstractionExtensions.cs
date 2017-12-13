using Android.Gms.Location;
using System;

namespace Plugin.ActivityRecognizer
{
    public static class AbstractionExtensions
    {
        public static MotionType GetMotionType(this DetectedActivity detectedActivity)
        {
            switch (detectedActivity.Type)
            {
                case DetectedActivity.Still:
                    return MotionType.Stationary;
                case DetectedActivity.OnFoot:
                    return MotionType.OnFoot;
                case DetectedActivity.Tilting:
                    return MotionType.Tilting;
                case DetectedActivity.Walking:
                    return MotionType.Walking;
                case DetectedActivity.Running:
                    return MotionType.Running;
                case DetectedActivity.OnBicycle:
                    return MotionType.OnBicycle;
                case DetectedActivity.InVehicle:
                    return MotionType.InVehicle;
                default:
                case DetectedActivity.Unknown:
                    return MotionType.Unknown;
            }
        }

        private static readonly DateTime ReferenceDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime GetDateTime(this ActivityRecognitionResult result)
        {
            return ReferenceDate.AddMilliseconds(result.Time);
        }
    }
}