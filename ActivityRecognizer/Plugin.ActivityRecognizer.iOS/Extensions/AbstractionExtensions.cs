using CoreMotion;
using Foundation;
using System;

namespace Plugin.ActivityRecognizer
{
    public static class AbstractionExtensions
    {
        public static MotionType GetMotionType(this CMMotionActivity motionActivity)
        {
            if (motionActivity.Automotive)
                return MotionType.InVehicle;
            else if (motionActivity.Cycling)
                return MotionType.OnBicycle;
            else if (motionActivity.Running)
                return MotionType.Running;
            else if (motionActivity.Walking)
                return MotionType.Walking;
            else if (motionActivity.Stationary)
                return MotionType.Stationary;
            else
                return MotionType.Unknown;
        }

        public static int ToPercentage(this CMMotionActivityConfidence confidence)
        {
            switch (confidence)
            {
                default:
                case CMMotionActivityConfidence.Low:
                    return 15;
                case CMMotionActivityConfidence.Medium:
                    return 50;
                case CMMotionActivityConfidence.High:
                    return 90;
            }
        }

        private static readonly DateTime ReferenceDate = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime ToDateTime(this NSDate date)
        {
            return ReferenceDate.AddSeconds(date.SecondsSinceReferenceDate);
        }
    }
}