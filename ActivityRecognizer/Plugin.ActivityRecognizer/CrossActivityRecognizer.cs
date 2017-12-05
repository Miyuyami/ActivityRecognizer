using System;
using System.Threading;

namespace Plugin.ActivityRecognizer
{
    /// <summary>
    /// Cross platform ActivityRecognizer implemenations
    /// </summary>
    public class CrossActivityRecognizer
    {
        private static Lazy<IActivityRecognizer> Implementation = new Lazy<IActivityRecognizer>(() => CreateActivityRecognizer(), LazyThreadSafetyMode.PublicationOnly);

        public static IActivityRecognizer Current
        {
            get
            {
                var ret = Implementation.Value;

                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }

                return ret;
            }
        }

        // TODO: config

        private static IActivityRecognizer CreateActivityRecognizer()
        {
#if WINDOWS_UWP || __IOS__
            return null;
#elif NETSTANDARD
            return null;
#else
            return new ActivityRecognizerImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
