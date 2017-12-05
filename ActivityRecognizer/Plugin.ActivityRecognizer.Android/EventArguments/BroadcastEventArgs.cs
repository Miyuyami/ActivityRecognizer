using Android.Content;
using System;

namespace Plugin.ActivityRecognizer
{
    public class BroadcastEventArgs : EventArgs
    {
        public Context Context { get; }
        public Intent Intent { get; }

        public BroadcastEventArgs(Context context, Intent intent)
        {
            this.Context = context;
            this.Intent = intent;
        }
    }
}