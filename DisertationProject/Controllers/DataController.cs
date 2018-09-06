using DisertationProject.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static DisertationProject.Model.Globals;

namespace DisertationProject.Controllers
{
    /// <summary>
    /// Data contoller
    /// </summary>
    public class DataController 
    {
        /// <summary>
        /// The data controller
        /// </summary>
        private static DataController _dataController;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DataController Instance
        {
            get
            {
                if (_dataController == null)
                    _dataController = new DataController();
                return _dataController;
            }
        }

        /// <summary>
        /// SQL connection
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// Sql data _reader
        /// </summary>
        private DataController()
        {
            EstablishConnection(Globals.ConnectionString);
        }

        /// <summary>
        /// Establish connection
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        private void EstablishConnection(string connectionString)
        {
            try
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
            };
        }

        /// <summary>
        /// Method to get all songs from the table
        /// </summary>
        /// <returns></returns>
        public GenericResponse<List<Song>> GetSongs()
        {
            var songList = new List<Song>();
            var response = new GenericResponse<List<Song>>
            {
                Status = Status.Success,
                Message = "Get songs success"
            };
            try
            {
                var _command = new SqlCommand("SELECT Id, Name, Artist, Source, Playlist FROM dbo.Songs", _connection);
                var _reader = _command.ExecuteReader();
                if (_reader.HasRows)
                {
                    // Read advances to the next row.
                    while (_reader.Read())
                    {
                        // To avoid unexpected bugs access columns by name.
                        songList.Add(new Song()
                        {
                            Id = _reader.GetInt32(_reader.GetOrdinal("Id")),
                            Title = _reader.GetString(_reader.GetOrdinal("Name")),
                            Artist = _reader.GetString(_reader.GetOrdinal("Artist")),
                            Source = Globals.FileStorage + _reader.GetString(_reader.GetOrdinal("Source"))
                        });
                    }
                }
                response.Result = songList;
            }
            catch (Exception ex)
            {
                response.Status = Status.Failed;
                response.Message = $"Get songs failed : {ex.Message}";
            };
            return response;
        }

        /// <summary>
        /// Gets the test songs.
        /// </summary>
        /// <returns></returns>
        public GenericResponse<List<Song>> GetTestSongs()
        {
            var songList = new List<Song>
            {
                new Song {Id = 101, Artist = "Trumpet", Title = "March", Group = Emotion.Happy, Source = Songs.SampleSong1},
                new Song {Id = 102, Artist = "Russia", Title = "Katyusha", Group = Emotion.Happy, Source = Songs.SampleSong2},
                new Song {Id = 103, Artist = "America", Title = "Yankee Doodle Dandy", Group = Emotion.Happy, Source = Songs.SampleSong3},
                new Song {Id = 104, Artist = "Romania", Title = "National Anthem", Group = Emotion.Happy, Source = Songs.SampleSong4}
            };
            var response = new GenericResponse<List<Song>>
            {
                Status = Status.Success,
                Message = "Get songs success",
                Result = songList
            };
            return response;
        }


        /// <summary>
        /// Method for connection close
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }
    }
}