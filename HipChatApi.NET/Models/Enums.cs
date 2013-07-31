
namespace HipChatApi.Models
{
    /// <summary>
    /// The currently supported background colors for messages in HipChat.
    /// </summary>
    public enum Color
    {
        Yellow,
        Red,
        Green,
        Purple,
        Gray,
        Random
    }

    public enum MessageFormat
    {
        Html,
        Text
    }

    public enum UserStatus
    {
        Offline,
        Away,
        Dnd,
        Available
    }

    public enum AccessLevel
    {
        Private,
        Public
    }
}
