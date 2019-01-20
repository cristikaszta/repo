using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using DisertationProject.Controllers;
using DisertationProject.Model;
using System;

namespace DisertationProject.Services
{
    /// <summary>
    /// Music service
    /// </summary>
    /// <seealso cref="Android.App.Service" />
    [Service]
    [IntentFilter(new[] { "ActionPlay",   "ActionPause", "ActionStop",
                          "ActionPrevious","ActionNext",
                          "ActionRepeatOn","ActionRepeatOff" })]
    public class MusicService : Service
    {
        /// <summary>
        /// Intent
        /// </summary>
        public Intent intent;

        /// <summary>
        /// Notification Id
        /// </summary>
        private const int notificationId = 1;

        /// <summary>
        /// The media player
        /// </summary>
        private MediaPlayer mediaPlayer;

        /// <summary>
        /// The audio manager
        /// </summary>
        private AudioManager audioManager;

        /// <summary>
        /// State
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public PlayState State { get; private set; }

        public override IBinder OnBind(Intent intent) { return null; }

        /// <summary>
        /// On create simply detect some of our managers
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            InitializeMediaPlayer();
        }

        /// <summary>
        /// Initializes the media player.
        /// </summary>
        private void InitializeMediaPlayer()
        {
            audioManager = (AudioManager)Application.Context.GetSystemService(AudioService);
            mediaPlayer = new MediaPlayer();

            mediaPlayer.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            mediaPlayer.SetWakeMode(Application.Context, WakeLockFlags.Partial);

            mediaPlayer.Prepared += (sender, args) => mediaPlayer.Start();

            mediaPlayer.Completion += (sender, args) =>
            {
                Intent intent = new Intent("GetNext");
                intent.PutExtra("GetNext", 0);
                SendBroadcast(intent);
            };

            mediaPlayer.Error += (sender, args) =>
            {
                Stop();
            };
        }

        /// <summary>
        /// On start command
        /// </summary>
        /// <param name="intent">The intent</param>
        /// <param name="flags">The start command flags</param>
        /// <param name="startId">Start id</param>
        /// <returns>
        /// Start command result
        /// </returns>
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            ActionEvent action = (ActionEvent)Enum.Parse(typeof(ActionEvent), intent.Action);
            switch (action)
            {
                case ActionEvent.ActionPlay:
                    var source = intent.GetStringExtra("source");
                    var title = intent.GetStringExtra("title");
                    var artist = intent.GetStringExtra("artist");
                    Play(source, title, artist);
                    break;

                case ActionEvent.ActionStop: Stop(); break;
                case ActionEvent.ActionPause: Pause(); break;
            }
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }

        /// <summary>
        /// Plays the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        private async void Play(string uri, string name, string artist)
        {
            if (!NetworkController.Instance.IsConnected)
            {
                Stop();
                return;
            }
            if (State == PlayState.Paused)
            {
                mediaPlayer.Start();
                State = PlayState.Playing;
                return;
            }

            if (mediaPlayer.IsPlaying) return;

            try
            {
                mediaPlayer.Reset();
                await mediaPlayer.SetDataSourceAsync(Application.Context, Android.Net.Uri.Parse(uri));
                mediaPlayer.PrepareAsync();
                NetworkController.Instance.AquireWifiLock();
                var text = string.Format("{0} - {1}", artist, name);
                StartForeground(text);
            }
            catch (Java.Lang.IllegalStateException ex)
            {
                Stop();
            }
        }

        /// <summary>
        /// Pause action method
        /// </summary>
        private void Pause()
        {
            if (mediaPlayer.IsPlaying)
            {
                mediaPlayer.Pause();
                State = PlayState.Paused;
            }
        }

        /// <summary>
        /// Stop action method
        /// </summary>
        private void Stop()
        {
            if (mediaPlayer.IsPlaying)
                mediaPlayer.Stop();
            mediaPlayer.Reset();
            State = PlayState.Stopped;
            StopForeground(true);
            NetworkController.Instance.ReleaseWifiLock();
        }

        /// <summary>
        /// Get the duration of the current song
        /// </summary>
        /// <returns>
        /// The duration of the current playing song
        /// </returns>
        private int GetSongDuration()
        {
            var duration = 0;
            try
            {
                duration = mediaPlayer.Duration;
            }
            catch (Exception ex)
            {
                Stop();
            };
            return duration;
        }

        /// <summary>
        /// Get the position of the current song playing
        /// </summary>
        /// <returns>
        /// The position of the current playing song
        /// </returns>
        public int GetCurrentPosition()
        {
            var position = 0;
            try
            {
                position = mediaPlayer.CurrentPosition;
            }
            catch (Exception)
            {
                Stop();
            }
            return position;
        }

        /// <summary>
        /// Properly cleanup of your player by releasing resources
        /// </summary>
        /// <remarks>
        /// <para tool="javadoc-to-mdoc">Called by the system to notify a Service that it is no longer used and is being removed.  The
        /// service should clean up any resources it holds (threads, registered
        /// receivers, etc) at this point.  Upon return, there will be no more calls
        /// in to this Service object and it is effectively dead.  Do not call this method directly.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Service.html#onDestroy()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para>
        /// </remarks>
        /// <since version="Added in API level 1" />
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (mediaPlayer != null)
            {
                mediaPlayer.Release();
                mediaPlayer = null;
            }
        }

        /// <summary>
        /// Start foreground
        /// When we start on the foreground we will present a notification to the user
        /// When they press the notification it will take them to the main page so they can control the music
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="song">The song.</param>
        private void StartForeground(string text)
        {
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, new Intent(Application.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
           
            var notification = new Notification
            {
                TickerText = new Java.Lang.String(text),
                Icon = Resource.Drawable.icon_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
            notification.SetLatestEventInfo(Application.Context, "Playing", text, pendingIntent);
            StartForeground(notificationId, notification);
        }
    }
}