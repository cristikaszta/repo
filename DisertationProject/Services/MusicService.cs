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
        private MediaPlayer MediaPlayer;

        /// <summary>
        /// The audio manager
        /// </summary>
        private AudioManager audioManager;

        /// <summary>
        /// The song name
        /// </summary>
        private string _songName = "SongName";

        /// <summary>
        /// The artist name
        /// </summary>
        private string _artistName = "ArtistName";

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
            //PlayList = new Playlist();
        }

        /// <summary>
        /// Initializes the media player.
        /// </summary>
        private void InitializeMediaPlayer()
        {
            audioManager = (AudioManager)Application.Context.GetSystemService(AudioService);
            MediaPlayer = new MediaPlayer();

            MediaPlayer.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            MediaPlayer.SetWakeMode(Application.Context, WakeLockFlags.Partial);

            MediaPlayer.Prepared += (sender, args) => MediaPlayer.Start();

            MediaPlayer.Completion += (sender, args) =>
            {
                Intent intent = new Intent("GetNext");
                intent.PutExtra("GetNext", 0);
                SendBroadcast(intent);
            };

            MediaPlayer.Error += (sender, args) =>
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
                    Play(source);
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
        private async void Play(string uri)
        {
            if (!NetworkController.Instance.IsConnected)
            {
                Stop();
                return;
            }
            if (State == PlayState.Paused)
            {
                MediaPlayer.Start();
                State = PlayState.Playing;
                //StartForeground();
                return;
            }

            if (MediaPlayer.IsPlaying) return;

            try
            {
                MediaPlayer.Reset();
                await MediaPlayer.SetDataSourceAsync(Application.Context, Android.Net.Uri.Parse(uri));
                MediaPlayer.PrepareAsync();
                NetworkController.Instance.AquireWifiLock();
                StartForeground("Playing ", _artistName, _songName);
                NetworkController.Instance.AquireWifiLock();
            }
            catch (Java.Lang.IllegalStateException ex)
            {
                Stop();
            }
        }

        /// <summary>
        /// Plays the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="name">The name.</param>
        /// <param name="artist">The artist.</param>
        private void Play(string uri, string name, string artist)
        {
            _songName = name;
            _artistName = artist;
            Play(uri);
        }

        /// <summary>
        /// Pause action method
        /// </summary>
        private void Pause()
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                State = PlayState.Paused;
            }
        }

        /// <summary>
        /// Stop action method
        /// </summary>
        private void Stop()
        {
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Stop();
            MediaPlayer.Reset();
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
                duration = MediaPlayer.Duration;
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
                position = MediaPlayer.CurrentPosition;
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
            if (MediaPlayer != null)
            {
                MediaPlayer.Release();
                MediaPlayer = null;
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
        private void StartForeground(string title, string artist, string song)
        {
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, new Intent(Application.Context, typeof(MainActivity)), PendingIntentFlags.UpdateCurrent);
            var text = string.Format("{0} - {1}", artist, song);
            var notification = new Notification
            {
                TickerText = new Java.Lang.String(text),
                Icon = Resource.Drawable.icon_play_over_video
            };

            notification.Flags |= NotificationFlags.OngoingEvent;
#pragma warning disable CS0618 // Type or member is obsolete
            notification.SetLatestEventInfo(Application.Context, "Playing", text, pendingIntent);
#pragma warning restore CS0618 // Type or member is obsolete
            StartForeground(notificationId, notification);
        }
    }
}