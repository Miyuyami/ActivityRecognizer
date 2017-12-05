using System;
using System.Collections.Generic;

namespace Plugin.ActivityRecognizer
{
    public class DetectedMotion : IEquatable<DetectedMotion>
    {
        public int Confidence { get; }
        public MotionType Type { get; }

        public DetectedMotion(int confidence, MotionType type)
        {
            this.Confidence = confidence;
            this.Type = type;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as DetectedMotion);
        }

        public bool Equals(DetectedMotion other)
        {
            return other != null &&
                   this.Confidence == other.Confidence &&
                   this.Type == other.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 2072953577;

                hashCode *= -1521134295 + this.Confidence.GetHashCode();
                hashCode *= -1521134295 + this.Type.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(DetectedMotion motion1, DetectedMotion motion2)
        {
            return EqualityComparer<DetectedMotion>.Default.Equals(motion1, motion2);
        }

        public static bool operator !=(DetectedMotion motion1, DetectedMotion motion2)
        {
            return !(motion1 == motion2);
        }
    }
}
