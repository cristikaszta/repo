namespace DisertationProject.Controllers
{
    /// <summary>
    /// Camera controller
    /// </summary>
    public class CameraController
    {
        /// <summary>
        /// The camera controller
        /// </summary>
        private static CameraController _cameraController;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CameraController Instance
        {
            get
            {
                if (_cameraController == null)
                    _cameraController = new CameraController();
                return _cameraController;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="CameraController"/> class from being created.
        /// </summary>
        private CameraController()
        {
        }

        public void TakePicture()
        {

        }
    }
}