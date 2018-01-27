
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisertationProject.Model
{
    /// <summary>
    /// Playlist class
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Playlist is
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Playlist name
        /// </summary>
        public string PlayListName { get; set; }

        /// <summary>
        /// The total items in the playlist
        /// </summary>
        private int _totalItems;

        /// <summary>
        /// The current position in the playlist
        /// </summary>
        private int _position;

        private List<Song> _songList;

        /// <summary>
        /// Suffle property
        /// </summary>
        public bool Shuffle { get; set; }

        /// <summary>
        /// Repeat property
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// Check if posiion is at end
        /// </summary>
        public bool IsAtEnd
        {
            get
            {
                if (_position == _totalItems - 1)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Check if position is at beggining
        /// </summary>
        public bool IsAtBeggining
        {
            get
            {
                if (_position == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <_parameter name="trackList">The tracklist</_parameter>
        public Playlist(List<Song> songList)
        {
            _position = 0;
            Shuffle = false;
            Repeat = false;
            _songList = songList;
            _totalItems = songList.Count;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(Song item)
        {
            _songList.Add(item);
            _totalItems++;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(List<Song> items)
        {
            _songList.AddRange(items);
            _totalItems += items.Count();
        }

        /// <summary>
        /// Remove items from playlist
        /// </summary>
        public void Remove(Song item)
        {
            if (_songList.Any())
            {
                _songList.Remove(item);
                _totalItems--;
            }
        }

        /// <summary>
        /// Get current song from the playlist
        /// </summary>
        /// <returns>Current song</returns>
        public Song GetCurrentSong()
        {
            return _songList[_position];
        }

        /// <summary>
        /// Increment current position
        /// </summary>
        public void IncrementPosition()
        {
            if (Shuffle)
            {
                var random = new Random();
                _position = random.Next(0, _totalItems - 1);
            }
            else if (_position < _totalItems - 1)
                _position++;
        }

        /// <summary>
        /// Decrement current position
        /// </summary>
        public void DecrementPosition()
        {
            if (Shuffle)
            {
                var random = new Random();
                _position = random.Next(0, _totalItems - 1);
            }
            else if (_position > 0)
                _position--;
        }

        /// <summary>
        /// Set position to zero (first position)
        /// </summary>
        public void ResetPosition()
        {
            _position = 0;
        }

        /// <summary>
        /// Set position to last
        /// </summary>
        public void SetPositionToEnd()
        {
            _position = _totalItems - 1;
        }

        public void SetPosition(int position)
        {
            if (0 <= position && position <= _totalItems)
                _position = position;
            else
                throw new InvalidOperationException("Position is not valid!");
        }

        internal List<Song> GetSongList()
        {
            return _songList;
        }
    }
}