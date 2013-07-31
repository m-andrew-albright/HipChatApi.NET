using System;
using System.Collections;
using System.Collections.Generic;

namespace HipChatApi.Models
{
    public class User
    {
        /// <summary>
        /// "API", "guest", or the integer ID of the given user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User's Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's full name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User's @mention name.
        /// </summary>
        public string MentionName { get; set; }

        /// <summary>
        /// User's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL to user's photo.  125px on the longest side.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Time the account was last used in UNIX time (UTC).  May be 0 in rare cases when the time is unknown.
        /// </summary>
        public DateTime LastActive { get; set; }

        /// <summary>
        /// Time the user was created in UNIX time (UTC).
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// User's current status.  Either offline, away, dnd, or available.
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// User's current status message.
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Whether or not this user is an admin of the group.
        /// </summary>
        public bool? IsGroupAdmin { get; set; }

        /// <summary>
        /// Whether this user has been deleted from the account.
        /// </summary>
        public bool? IsDeleted { get; set; }

        internal UserRaw ToUserRaw()
        {
            return new UserRaw
            {
                user_id = UserId,
                email = Email,
                name = Name,
                mention_name = MentionName,
                title = Title,
                photo = AvatarUrl,
                last_active = LastActive,
                created = Created,
                status = Status.ToString().ToLowerInvariant(),
                status_message = StatusMessage,
                is_group_admin = (IsGroupAdmin ?? false) ? 1 : 0,
                is_deleted = (IsDeleted ?? false) ? 1 : 0
            };
        }
    }

    internal class UserRaw
    {
        /// <summary>
        /// User's ID.  Usually an integer, unless a message was sent via "API" or a "guest".
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// User's Email.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// User's full name.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// User's @mention name.
        /// </summary>
        public string mention_name { get; set; }

        /// <summary>
        /// User's title.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// URL to user's photo.  125px on the longest side.
        /// </summary>
        public string photo { get; set; }

        /// <summary>
        /// Time the account was last used in UNIX time (UTC).  May be 0 in rare cases when the time is unknown.
        /// </summary>
        public DateTime last_active { get; set; }

        /// <summary>
        /// Time the user was created in UNIX time (UTC).
        /// </summary>
        public DateTime created { get; set; }

        /// <summary>
        /// User's current status.  Either offline, away, dnd, or available.
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// User's current status message.
        /// </summary>
        public string status_message { get; set; }

        /// <summary>
        /// Whether or not this user is an admin of the group.
        /// </summary>
        public int is_group_admin { get; set; }

        public int is_deleted { get; set; }

        /// <summary>
        /// The user's unhashed password.
        /// </summary>
        public string password { get; set; }

        //TODO 1: TimeZone handling...

        public User ToUser()
        {
            return new User
            {
                UserId = user_id,
                Email = email,
                Name = name,
                MentionName = mention_name,
                Title = title,
                AvatarUrl = photo,
                LastActive = last_active,
                Created = created,
                Status = (UserStatus)Enum.Parse(typeof(UserStatus), status ?? "offline", true),
                StatusMessage = status_message,
                IsGroupAdmin = is_group_admin == 1,
                IsDeleted = is_deleted == 1
            };
        }
    }

    internal class UsersRaw : IEnumerable<UserRaw>
    {
        public List<UserRaw> users { get; set; }

        public IEnumerator<UserRaw> GetEnumerator()
        {
            return users.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
