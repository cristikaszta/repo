using Android.Content;
using System;

namespace DisertationProject
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Android.Content.BroadcastReceiver" />
    [BroadcastReceiver]
    public class SongCompletionReceiver : BroadcastReceiver
    {
        /// <summary>
        /// Occurs when [song completion event handler].
        /// </summary>
        public event EventHandler SongCompletionEventHandler;

        /// <summary>
        /// This method is called when the BroadcastReceiver is receiving an Intent
        /// broadcast.
        /// </summary>
        /// <param name="context">The Context in which the receiver is running.</param>
        /// <param name="intent">The Intent being received.</param>
        public override void OnReceive(Context context, Intent intent)
        {
            var e = new EventArgs();
            SongCompletionEventHandler?.Invoke(this, e);
        }
    }
}