using Android.Content;
using System;

namespace Plugin.ActivityRecognizer
{
    internal sealed class EventBroadcastReceiver : BroadcastReceiver
    {
        public event EventHandler<BroadcastEventArgs> Received;

        public override void OnReceive(Context context, Intent intent)
        {
            this.Received?.Invoke(this, new BroadcastEventArgs(context, intent));
        }
    }
}