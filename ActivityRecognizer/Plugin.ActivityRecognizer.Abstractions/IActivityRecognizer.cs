using System;

namespace Plugin.ActivityRecognizer
{
    /// <summary>
    /// Interface for ActivityRecognizer
    /// </summary>
    public interface IActivityRecognizer
    {
        bool IsSupported { get; }

        /// <summary>
        /// Monitor for all motions received from the device.
        /// </summary>
        /// <returns></returns>
        IObservable<DetectedMotionResult> WhenMotionDetected();

        /// <summary>
        /// Returns the last detected motion, if none, it will return when one is detected.
        /// </summary>
        /// <returns></returns>
        IObservable<DetectedMotionResult> GetMotion();
    }
}
