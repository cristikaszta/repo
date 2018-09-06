namespace DisertationProject.Model
{
    /// <summary>
    /// Song model
    /// </summary>
    public class Song
    {
        /// <summary>
        /// Id of the song
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Name of the song
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Source of the song
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Artist name
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist { get; set; }

        /// <summary>
        /// Emotion
        /// </summary>
        /// <value>
        /// The emotion.
        /// </value>
        public Emotion Group { get; set; }
    }
}