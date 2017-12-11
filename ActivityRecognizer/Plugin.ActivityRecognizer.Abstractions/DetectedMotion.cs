using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Plugin.ActivityRecognizer
{
    [DebuggerDisplay("DebuggerDisplay,nq")]
    public class DetectedMotion : IEquatable<DetectedMotion>, IComparable<DetectedMotion>
    {
        public int Confidence { get; }
        public MotionType Type { get; }

        public DetectedMotion(int confidence, MotionType type)
        {
            this.Confidence = confidence;
            this.Type = type;
        }

        public int CompareTo(DetectedMotion other)
        {
            return this.Confidence.CompareTo(other.Confidence);
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

        public override string ToString()
        {
            return $"{this.Type} with {this.Confidence}% confidence.";
        }

        public static bool operator ==(DetectedMotion motion1, DetectedMotion motion2)
        {
            return EqualityComparer<DetectedMotion>.Default.Equals(motion1, motion2);
        }

        public static bool operator !=(DetectedMotion motion1, DetectedMotion motion2)
        {
            return !(motion1 == motion2);
        }

        private string DebuggerDisplay
            => $"{this.Type} - {this.Confidence}%";
    }
}
