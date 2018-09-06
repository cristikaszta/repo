namespace DisertationProject.Model
{
    /// <summary>
    ///
    /// </summary>
    public class GenericResponse
    {
        /// <summary>
        /// The status
        /// </summary>
        public Status Status;

        /// <summary>
        /// The message
        /// </summary>
        public string message;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericResponse<T>
    {
        /// <summary>
        /// The status
        /// </summary>
        public Status Status;

        /// <summary>
        /// The message
        /// </summary>
        public string Message;

        /// <summary>
        /// The result
        /// </summary>
        public T Result;
    }
}