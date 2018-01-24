using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DisertationProject
{
    [BroadcastReceiver]
    public class SongCompletionReceiver : BroadcastReceiver
    {
        public event EventHandler SongCompletionEventHandler;

        public override void OnReceive(Context context, Intent intent)
        {
            var e = new EventArgs();
            SongCompletionEventHandler?.Invoke(this, e);
        }
    }
}