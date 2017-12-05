using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.ActivityRecognizer
{
    public class DetectedMotionResult : IEquatable<DetectedMotionResult>
    {
        public DateTime Timestamp { get; }
        public DetectedMotion MostProbableMotion { get; }
        public IReadOnlyList<DetectedMotion> DetectedMotions { get; }

        public DetectedMotionResult(DateTime timestamp, DetectedMotion mostProbableMotion, DetectedMotion[] detectedMotions)
        {
            this.Timestamp = timestamp;
            this.MostProbableMotion = mostProbableMotion;
            this.DetectedMotions = Array.AsReadOnly(detectedMotions);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as DetectedMotionResult);
        }

        public bool Equals(DetectedMotionResult other)
        {
            return other != null &&
                   this.Timestamp == other.Timestamp &&
                   this.MostProbableMotion == other.MostProbableMotion &&
                   this.DetectedMotions.SequenceEqual(other.DetectedMotions, EqualityComparer<DetectedMotion>.Default);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 2072718625;

                hashCode *= -1521134295 + this.Timestamp.GetHashCode();
                hashCode *= -1521134295 + this.MostProbableMotion.GetHashCode();
                hashCode *= -1521134295 + this.DetectedMotions.Aggregate(-1113886543, (hc, dm) => hc * -1521134295 + dm?.GetHashCode() ?? -2120674293);

                return hashCode;
            }
        }

        public static bool operator ==(DetectedMotionResult result1, DetectedMotionResult result2)
        {
            return EqualityComparer<DetectedMotionResult>.Default.Equals(result1, result2);
        }

        public static bool operator !=(DetectedMotionResult result1, DetectedMotionResult result2)
        {
            return !(result1 == result2);
        }
    }
}
