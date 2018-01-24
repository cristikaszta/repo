using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using DisertationProject.Adapters;
using DisertationProject.Controller;
using DisertationProject.Model;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Timers;
using static DisertationProject.Model.Globals;

namespace DisertationProject
{
    [Activity(Label = "DisertationProject", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private ImageView _imageView;

        /// <summary>
        /// Textview object which containes the text
        /// </summary>
        private TextView _textView;

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
        public Playlist _playList { get; set; }

        private SongCompletionReceiver _songCompletionReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            //GetSongList();
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
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source);
            };

            RegisterReceiver(_songCompletionReceiver, new IntentFilter("GetNext"));
        }
        private void GetSongList()
        {
            var response = DataController.Instance.GetSongs();
            if (response.Status == GenericStatus.Success)
            {
                _playList = new Playlist
                {
                    SongList = response.Result
                };
            };
        }

        /// <summary>
        /// Setup playlist
        /// </summary>
        private void SetupPlaylist()
        {
            var songList = new List<Song>
            {
                new Song {Id = 101, Artist = "Trumpet", Name = "March", Emotion = Emotion.Happy, Source = Songs.SampleSong1},
                new Song {Id = 102, Artist = "Russia", Name = "Katyusha", Emotion = Emotion.Happy, Source = Songs.SampleSong2},
                new Song {Id = 103, Artist = "America", Name = "Yankee Doodle Dandy", Emotion = Emotion.Happy, Source = Songs.SampleSong3},
                new Song {Id = 104, Artist = "Romania", Name = "National Anthem", Emotion = Emotion.Happy, Source = Songs.SampleSong4}
            };

            _playList = new Playlist(songList);

        }

        private void ChangeColorOfButton(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            button.SetBackgroundColor(Color.Blue);
            Timer timer = new Timer(300);
            timer.Elapsed += (s, e) => { button.SetBackgroundColor(Color.Blue); };
            timer.Start();
        }

        private void SetupButtons()
        {
            var playButton = FindViewById<Button>(Resource.Id.playButton);
            playButton.Click += (sender, args) =>
            {
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source, name);
            };

            playButton.Click += ChangeColorOfButton;

           

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
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionStop);
                SendCommand(ActionEvent.ActionPlay, source, name);
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
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionStop);
                SendCommand(ActionEvent.ActionPlay, source, name);
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
            _textView = FindViewById<TextView>(Resource.Id.textView1);
            _listView = FindViewById<ListView>(Resource.Id.songListView);
            _songListAdapter = new SonglistAdapter(this, _playList.SongList);
            _listView.Adapter = _songListAdapter;
            _listView.ItemClick += (sender, args) =>
            {
                SendCommand(ActionEvent.ActionStop);
                var currentPosition = args.Position;
                _playList.Position = currentPosition;
                var name = _playList.GetCurrentSong().Name;
                var source = _playList.GetCurrentSong().Source;
                SendCommand(ActionEvent.ActionPlay, source, name);
            };
        }

        /// <summary>
        /// Send command
        /// </summary>
        /// <param name="action">The intended ation</param>
        public void SendCommand(ActionEvent action)
        {
            var stringifiedAction = Helper.ConvertActionEvent(action);
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicService));
            StartService(intent);
        }

        public void SendCommand(ActionEvent action, string source)
        {
            var stringifiedAction = Helper.ConvertActionEvent(action);
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicService));
            intent.PutExtra("source", source);
            StartService(intent);
        }

        public void SendCommand(ActionEvent action, string source, string name)
        {
            var stringifiedAction = Helper.ConvertActionEvent(action);
            var intent = new Intent(stringifiedAction, null, this, typeof(MusicService));
            intent.PutExtra("source", source);
            intent.PutExtra("name", name);
            StartService(intent);
        }
    }
}

