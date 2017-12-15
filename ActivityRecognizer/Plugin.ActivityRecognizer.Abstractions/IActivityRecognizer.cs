using System;

namespace Plugin.ActivityRecognizer
{
    /// <summary>
    /// Interface for ActivityRecognizer
    /// </summary>
    public interface IActivityRecognizer
    {
        /// <summary>
        /// Returns true if activity recognition is supported on the device, otherwise false.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// An observable that will monitor all detected motions.
        /// </summary>
        /// <returns></returns>
        IObservable<DetectedMotionResult> WhenMotionDetected();

        /// <summary>
        /// Returns an <see cref="IObservable{T}"/> containing the last detected motion as <see cref="DetectedMotionResult"/>, if one has yet to be detected, the result will be sent when the motion is detected.
        /// </summary>
        /// <returns>An <see cref="IObservable{T}"/> containing the result, <see cref="DetectedMotionResult"/>.</returns>
        IObservable<DetectedMotionResult> GetLastMotion();
    }
}
