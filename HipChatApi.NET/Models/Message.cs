using System;
using System.Collections;
using System.Collections.Generic;

namespace HipChatApi.Models
{
    public class Message
    {
        /// <summary>
        /// ID or name of the room.
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// Name the message will appear to be sent from.  Must be less than 15 characters long.  
        /// May contain letters, numbers, ', _, and spaces.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The message body.  10,000 characters max.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Determines how the message is treated by the server and rendered inside HipChat applications.
        /// 
        /// - html: Message is rendered as HTML and receives no special treatment.  Must be
        ///   valid HTML and entities must be escaped (e.g.: &amp; instead of &). May contain basic
        ///   tags: a, b, i, strong, em, br, img, pre, code, lists, tables. Special HipChat features such 
        ///   as @mentions, emoticons, and image previews are NOT supported when using this format.
        /// 
        /// - text: Message is treated just like a message sent by a user. Can include @mentions, 
        ///   emoticons, pastes, and auto-detected URLs (Twitter, YouTube, images, etc).
        /// 
        /// (default: html)
        /// </summary>
        public MessageFormat? MessageFormat { get; set; }

        /// <summary>
        /// Whether or not this message should trigger a notification for people in the room (change the 
        /// tab color, play a sound, etc). Each recipient's notification preferences are taken into 
        /// account. 0 = false, 1 = true. (default: 0)
        /// </summary>
        public int? Notify { get; set; }

        /// <summary>
        /// Background color for message. One of "yellow", "red", "green", "purple", "gray", or "random". 
        /// (default: yellow)
        /// </summary>
        public Color? Color { get; set; }

        /// <summary>
        /// Date message was sent.
        /// </summary>
        public DateTime TimeSent { get; set; }

        /// <summary>
        /// Name and user_id of sender.  user_id will be "api" for API messages and "guest" for guest messages.
        /// </summary>
        public User SendingUser { get; set; }

        /// <summary>
        /// Name, size, and URL of uploaded file.
        /// </summary>
        public File File { get; set; }

        internal OutgoingMessageRaw ToOutgoing()
        {
            return new OutgoingMessageRaw
            {
                room_id = RoomId,
                from = From,
                message = Text,
                message_format = MessageFormat.ToString().ToLowerInvariant(),
                notify = Notify,
                color = Color.ToString().ToLowerInvariant()
            };
        }
    }

    internal class OutgoingMessageRaw
    {
        /// <summary>
        /// ID or name of the room.
        /// </summary>
        public string room_id { get; set; }

        /// <summary>
        /// Name the message will appear to be sent from.  Must be less than 15 characters long.  
        /// May contain letters, numbers, ', _, and spaces.
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// The message body.  10,000 characters max.
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Determines how the message is treated by the server and rendered inside HipChat applications.
        /// 
        /// - html: Message is rendered as HTML and receives no special treatment.  Must be
        ///   valid HTML and entities must be escaped (e.g.: &amp; instead of &). May contain basic
        ///   tags: a, b, i, strong, em, br, img, pre, code, lists, tables. Special HipChat features such 
        ///   as @mentions, emoticons, and image previews are NOT supported when using this format.
        /// 
        /// - text: Message is treated just like a message sent by a user. Can include @mentions, 
        ///   emoticons, pastes, and auto-detected URLs (Twitter, YouTube, images, etc).
        /// 
        /// (default: html)
        /// </summary>
        public string message_format { get; set; }

        /// <summary>
        /// Whether or not this message should trigger a notification for people in the room (change the 
        /// tab color, play a sound, etc). Each recipient's notification preferences are taken into 
        /// account. 0 = false, 1 = true. (default: 0)
        /// </summary>
        public int? notify { get; set; }

        /// <summary>
        /// Background color for message. One of "yellow", "red", "green", "purple", "gray", or "random". 
        /// (default: yellow)
        /// </summary>
        public string color { get; set; }
    }

    internal class IncomingMessageRaw
    {
        /// <summary>
        /// Date message was sent in ISO-8601 format in request timezone.
        /// </summary>
        public DateTime? date { get; set; }

        /// <summary>
        /// Name and user_id of sender.  user_id will be "api" for API messages and "guest" for guest messages.
        /// </summary>
        public UserRaw from { get; set; }

        /// <summary>
        /// Message body.
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Name, size, and URL of uploaded file.
        /// </summary>
        public FileRaw file { get; set; }

        public Message ToMessage()
        {
            return new Message
                {
                    TimeSent = date ?? DateTime.MinValue,
                    SendingUser = from != null ? from.ToUser() : null,
                    Text = message,
                    File = file != null ? file.ToFile() : null
                };
        }
    }

    internal class IncomingMessagesRaw : IEnumerable<IncomingMessageRaw>
    {
        public List<IncomingMessageRaw> messages { get; set; }

        public IEnumerator<IncomingMessageRaw> GetEnumerator()
        {
            return messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
