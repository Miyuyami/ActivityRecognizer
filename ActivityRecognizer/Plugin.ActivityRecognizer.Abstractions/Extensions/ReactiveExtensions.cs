using System;

namespace Plugin.ActivityRecognizer.Extensions
{
    public static class ReactiveExtensions
    {
        public static void Respond<T>(this IObserver<T> ob, T value)
        {
            ob.OnNext(value);
            ob.OnCompleted();
        }
    }
}
