using DisertationProject.Model;

namespace DisertationProject.Helpers
{
    /// <summary>
    /// Helper class
    /// </summary>
    public class Helper
    {
        #region Converters

        /// <summary>
        /// Convert string to emotion enum
        /// </summary>
        /// <param name="str">String text</param>
        /// <returns>
        /// Emotion enum
        /// </returns>
        public static Emotion ConvertEmotion(string str)
        {
            Emotion result;
            switch (str)
            {
                case "H": result = Emotion.Happy; break;
                case "A": result = Emotion.Angry; break;
                case "S": result = Emotion.Sad; break;
                case "N": result = Emotion.Neutral; break;
                default: result = Emotion.Neutral; break;
            }
            return result;
        }

        /// <summary>
        /// Convert emotion enum to string text
        /// </summary>
        /// <param name="emotion">Emotion enum value</param>
        /// <returns>
        /// String text
        /// </returns>
        public static string ConvertEmotion(Emotion emotion)
        {
            string result;
            switch (emotion)
            {
                case Emotion.Happy: result = "H"; break;
                case Emotion.Angry: result = "A"; break;
                case Emotion.Sad: result = "S"; break;
                case Emotion.Neutral: result = "N"; break;
                default: result = "N"; break;
            }
            return result;
        }

        ///// <summary>
        ///// Convert action event enum to string text
        ///// </summary>
        ///// <param name="actionEvent">ActionEvent enum value</param>
        ///// <returns>
        ///// String text
        ///// </returns>
        //public static ActionEvent ConvertActionEvent(string actionEvent)
        //{
        //    ActionEvent result;
        //    switch (actionEvent)
        //    {
        //        case "ActionPlay": result = ActionEvent.ActionPlay; break;
        //        case "ActionStop": result = ActionEvent.ActionStop; break;
        //        case "ActionPause": result = ActionEvent.ActionPause; break;
        //        case "ActionPrevious": result = ActionEvent.ActionPrevious; break;
        //        case "ActionNext": result = ActionEvent.ActionNext; break;
        //        case "ActionRepeatOn": result = ActionEvent.ActionRepeatOn; break;
        //        case "ActionRepeatOff": result = ActionEvent.ActionRepeatOff; break;
        //        case "ActionShuffleOn": result = ActionEvent.ActionShuffleOn; break;
        //        case "ActionShuffleOff": result = ActionEvent.ActionShuffleOff; break;
        //        default: result = ActionEvent.ActionStop; break;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Convert action event enum to string text
        ///// </summary>
        ///// <param name="actionEvent">ActionEvent enum value</param>
        ///// <returns>
        ///// String text
        ///// </returns>
        //public static string ConvertActionEvent(ActionEvent actionEvent)
        //{
        //    string result;
        //    switch (actionEvent)
        //    {
        //        case ActionEvent.ActionPlay: result = "ActionPlay"; break;
        //        case ActionEvent.ActionStop: result = "ActionStop"; break;
        //        case ActionEvent.ActionPause: result = "ActionPause"; break;
        //        case ActionEvent.ActionPrevious: result = "ActionPrevious"; break;
        //        case ActionEvent.ActionNext: result = "ActionNext"; break;
        //        case ActionEvent.ActionRepeatOn: result = "ActionRepeatOn"; break;
        //        case ActionEvent.ActionRepeatOff: result = "ActionRepeatOff"; break;
        //        case ActionEvent.ActionShuffleOn: result = "ActionShuffleOn"; break;
        //        case ActionEvent.ActionShuffleOff: result = "ActionShuffleOff"; break;
        //        default: result = "ActionStop"; break;
        //    }
        //    return result;
        //}

        #endregion Converters
    }
}