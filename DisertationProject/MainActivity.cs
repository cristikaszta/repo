using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Widget;
using CameraAppDemo;
using DisertationProject.Adapters;
using DisertationProject.Controllers;
using DisertationProject.Helpers;
using DisertationProject.Model;
using DisertationProject.Services;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Timers;
using static DisertationProject.Model.Globals;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace DisertationProject
{
    /// <summary>
    /// Main Activity
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    [Activity(Label = "DisertationProject", MainLauncher = true, Icon = "@mipmap/icon")]
    public sealed class MainActivity : Activity
    {
        /// <summary>
        /// The image view
        /// </summary>
        private ImageView _imageView;

        /// <summary>
        /// Textview object which containes the text
        /// </summary>
        private TextView _textView;

        private int _pictureCount;

        /// <summary>
        /// Song list adapter
        /// </summary>
        private SonglistAdapter _songListAdapter;

        /// <summary>
        /// The list view
        /// </summary>
        private ListView _listView;

        /// <summary>
        /// Playlist
        /// </summary>
        /// <value>
        /// The playlist.
        /// </value>
        private Playlist _playList;

        /// <summary>
        /// The song completion receiver
        /// </summary>
        private SongCompletionReceiver _songCompletionReceiver;

        private readonly string[] _permissions =
        {
            Manifest.Permission.Camera,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
        };

        /// <summary>
        /// Called when an activity you launched exits, giving you the requestCode
        /// you started it with, the resultCode it returned, and any additional
        /// data from it.
        /// </summary>
        /// <param name="requestCode">The integer request code originally supplied to
        /// startActivityForResult(), allowing you to identify who this
        /// result came from.</param>
        /// <param name="resultCode">The integer result code returned by the child activity
        /// through its setResult().</param>
        /// <param name="data">An Intent, which can return result data to the caller
        /// (various data can be attached to Intent "extras").</param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Width;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                _imageView.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        /// <summary>
        /// Creates the directory for pictures.
        /// </summary>
        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        /// <summary>
        /// Determines whether [is there an application to take pictures].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is there an application to take pictures]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        /// <summary>
        /// Takes a picture.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            App._file = new File(App._dir, string.Format("myPhoto_{0}_{1}.jpg", DateTime.Now.ToString("yyyy_mm_dd"), _pictureCount++));

            Uri imageUri = Android.Support.V4.Content.FileProvider.GetUriForFile(
            this,
            "DisertationProject.provider", //(use your app signature + ".provider" )
            App._file);

            intent.PutExtra(MediaStore.ExtraOutput, imageUri);

            if (CheckSelfPermission(_permissions[0]) == (int)Permission.Granted)
            {

                StartActivityForResult(intent, 0);

            }
        }

        /// <summary>
        /// Called when the activity is starting.
        /// </summary>
        /// <param name="savedInstanceState">If the activity is being re-initialized after
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            SetupPlaylist();
            SetupButtons();
            SetupTextContainers();
            _songCompletionReceiver = new SongCompletionReceiver();
            _songCompletionReceiver.SongCompletionEventHandler += (sender, args) =>
            {
                if (!_playList.IsAtEnd)
                {
                    _playList.IncrementPosition();
                }
                else if (_playList.Repeat)
                {
                    _playList.ResetPosition();
                }
                else
                {
                    return;
                }
                _playList.IncrementPosition();
                var song = _playList.GetCurrentSong();
                SendCommand(ActionEvent.ActionPlay, song);
            };

            RegisterReceiver(_songCompletionReceiver, new IntentFilter("GetNext"));

            RequestPermissions(_permissions, 0);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button button = FindViewById<Button>(Resource.Id.cameraButton);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);

                button.Click += TakeAPicture;

            }
        }

        /// <summary>
        /// Gets the song list.
        /// </summary>
        private void GetSongList()
        {
            var response = DataController.Instance.GetSongs();
            if (response.Status == Status.Success)
            {
                _playList = new Playlist(response.Result);
            };
        }


        /// <summary>
        /// Setup playlist
        /// </summary>
        private void SetupPlaylist()
        {
            var songList = DataController.Instance.GetTestSongs().Result;

            _playList = new Playlist(songList);
        }

        /// <summary>
        /// Changes the color of button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ChangeColorOfButton(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            button.SetBackgroundColor(Color.Blue);
            Timer timer = new Timer(300);
            timer.Elapsed += (s, e) => { button.SetBackgroundColor(Color.Blue); };
            timer.Start();
        }

        /// <summary>
        /// Setups the buttons.
        /// </summary>
        private void SetupButtons()
        {
            var playButton = FindViewById<Button>(Resource.Id.playButton);
            playButton.Click += (sender, args) =>
            {
                var song = _playList.GetCurrentSong();
                SendCommand(ActionEvent.ActionPlay, song);
            };

            var pauseButton = FindViewById<Button>(Resource.Id.pauseButton);
            pauseButton.Click += (sender, args) => SendCommand(ActionEvent.ActionPause);

            var stopButton = FindViewById<Button>(Resource.Id.stopButton);
            stopButton.Click += (sender, args) => SendCommand(ActionEvent.ActionStop);

            var previousButton = FindViewById<Button>(Resource.Id.previousButton);
            previousButton.Click += (sender, args) =>
            {
                if (!_playList.IsAtBeggining)
                {
                    _playList.DecrementPosition();
                }
                else if (_playList.Repeat)
                {
                    _playList.SetPositionToEnd();
                }
                else
                {
                    return;
                }
                var song = _playList.GetCurrentSong();
                SendCommand(ActionEvent.ActionStop);
                SendCommand(ActionEvent.ActionPlay, song);
            };

            var nextButton = FindViewById<Button>(Resource.Id.nextButton);
            nextButton.Click += (sender, args) =>
            {
                if (!_playList.IsAtEnd)
                {
                    _playList.IncrementPosition();
                }
                else if (_playList.Repeat)
                {
                    _playList.ResetPosition();
                }
                else
                {
                    return;
                }
                var song = _playList.GetCurrentSong();
                SendCommand(ActionEvent.ActionPlay, song);
            };

            var repeatButton = FindViewById<ToggleButton>(Resource.Id.repeatButton);
            repeatButton.Click += (sender, args) =>
            {
                if (repeatButton.Checked)
                    _playList.Repeat = true;
                else
                    _playList.Repeat = false;
            };

            var shuffleButton = FindViewById<ToggleButton>(Resource.Id.shuffleButton);
            shuffleButton.Click += (sender, args) =>
            {
                if (shuffleButton.Checked)
                    _playList.Shuffle = true;
                else
                    _playList.Shuffle = false;
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void SetupTextContainers()
        {
            _listView = FindViewById<ListView>(Resource.Id.songListView);
            _songListAdapter = new SonglistAdapter(this, _playList.GetSongList());
            _listView.Adapter = _songListAdapter;
            _listView.ItemClick += (sender, args) =>
            {
                SendCommand(ActionEvent.ActionStop);
                var currentPosition = args.Position;
                _playList.SetPosition(currentPosition);
                var song = _playList.GetCurrentSong();
                SendCommand(ActionEvent.ActionPlay, song);
            };
        }

        /// <summary>
        /// Send command
        /// </summary>
        /// <param name="action">The intended ation</param>
        public void SendCommand(ActionEvent action)
        {
            //var stringifiedAction = Helper.ConvertActionEvent(action);
            var stringifiedAction = action.ToString();
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicService));
            StartService(intent);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="source">The source.</param>
        public void SendCommand(ActionEvent action, string source)
        {
            //var stringifiedAction = Helper.ConvertActionEvent(action);
            var stringifiedAction = action.ToString();
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicService));
            intent.PutExtra("source", source);
            StartService(intent);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="source">The source.</param>
        /// <param name="name">The name.</param>
        public void SendCommand(ActionEvent action, Song song)
        {
            //var stringifiedAction = Helper.ConvertActionEvent(action);
            var stringifiedAction = action.ToString();
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicService));
            intent.PutExtra("source", song.Source);
            intent.PutExtra("title", song.Title);
            intent.PutExtra("artist", song.Artist);
            StartService(intent);
        }
    }
}