using System.ComponentModel;

namespace DisertationProject.Model
{
    /// <summary>
    /// 
    /// </summary>
    public enum ActionEvent
    {
        /// <summary>
        /// The action play
        /// </summary>
        [Description("PLAY")]
        ActionPlay,
        /// <summary>
        /// The action pause
        /// </summary>
        [Description("PAUSE")]
        ActionPause,
        /// <summary>
        /// The action stop
        /// </summary>
        [Description("ActionSTOP")]
        ActionStop,
        /// <summary>
        /// The action previous
        /// </summary>
        [Description("PREVIOUS")]
        ActionPrevious,
        /// <summary>
        /// The action next
        /// </summary>
        [Description("NEXT")]
        ActionNext,
        /// <summary>
        /// The action repeat on
        /// </summary>
        [Description("REPEAT_ON")]
        ActionRepeatOn,
        /// <summary>
        /// The action repeat off
        /// </summary>
        [Description("REPEAT_OFF")]
        ActionRepeatOff,
        /// <summary>
        /// The action shuffle on
        /// </summary>
        [Description("SHUFFLE_ON")]
        ActionShuffleOn,
        /// <summary>
        /// The action shuffle off
        /// </summary>
        [Description("SHUFFLE_OFF")]
        ActionShuffleOff
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Emotion
    {
        /// <summary>
        /// The sad
        /// </summary>
        [Description("Sad")]
        Sad,
        /// <summary>
        /// The happy
        /// </summary>
        [Description("Happy")]
        Happy,
        /// <summary>
        /// The neutral
        /// </summary>
        [Description("Neutral")]
        Neutral,
        /// <summary>
        /// The angry
        /// </summary>
        [Description("Angry")]
        Angry
    }
    /// <summary>
    /// 
    /// </summary>
    public enum TextType
    {
        /// <summary>
        /// The information
        /// </summary>
        [Description("Info")]
        Info,
        /// <summary>
        /// The warning
        /// </summary>
        [Description("Warning")]
        Warning,
        /// <summary>
        /// The error
        /// </summary>
        [Description("Error")]
        Error
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// The playing
        /// </summary>
        [Description("Playing")]
        Playing,
        /// <summary>
        /// The stopped
        /// </summary>
        [Description("Stopped")]
        Stopped,
        /// <summary>
        /// The paused
        /// </summary>
        [Description("Paused")]
        Paused
    }

}