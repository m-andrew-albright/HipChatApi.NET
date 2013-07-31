using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HipChatApi.Models
{
    public class Room
    {
        /// <summary>
        /// ID of the room.
        /// </summary>
        public int? RoomId { get; set; }

        /// <summary>
        /// Name of the room.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current topic.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Time of last activity (sent message) in the room in UNIX time (UTC).
        /// </summary>
        public DateTime? LastActive { get; set; }

        /// <summary>
        /// Time the room was created in UNIX time (UTC).
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Whether or not this room is archived.
        /// </summary>
        public bool? IsArchived { get; set; }

        /// <summary>
        /// Whether or not this room is private.
        /// </summary>
        public bool? IsPrivate { get; set; }

        /// <summary>
        /// User ID of the room owner.
        /// </summary>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Privacy setting (public/private).
        /// </summary>
        public AccessLevel? AccessLevel { get; set; }

        /// <summary>
        /// Array containing user IDs and names of current room participants.
        /// </summary>
        public List<User> Participants { get; set; }

        /// <summary>
        /// Whether guests are allowed to participate in the chat.
        /// </summary>
        public bool? AllowsGuests { get; set; }

        /// <summary>
        /// URL for guest access, if enabled.
        /// </summary>
        public string GuestAccessUrl { get; set; }

        /// <summary>
        /// XMPP/Jabber ID of the room.
        /// </summary>
        public string XmppJabberId { get; set; }

        internal RoomRaw ToRoomRaw()
        {
            return new RoomRaw
            {
                room_id = RoomId,
                guest_access = (AllowsGuests ?? false) ? 1 : 0,
                name = Name,
                topic = Topic,
                last_active = LastActive,
                created = Created,
                is_archived = IsArchived,
                is_private = IsPrivate,
                owner_user_id = OwnerId,
                privacy = AccessLevel.ToString().ToLowerInvariant(),
                participants = Participants.SelectSafely(u => u.ToUserRaw()).ToList(),
                guest_access_url = GuestAccessUrl,
                xmpp_jid = XmppJabberId
            };
        }
    }

    internal class RoomRaw
    {
        /// <summary>
        /// ID of the room.
        /// </summary>
        public int? room_id { get; set; }

        /// <summary>
        /// Name of the room.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Current topic.
        /// </summary>
        public string topic { get; set; }

        /// <summary>
        /// Time of last activity (sent message) in the room in UNIX time (UTC).
        /// </summary>
        public DateTime? last_active { get; set; }

        /// <summary>
        /// Time the room was created in UNIX time (UTC).
        /// </summary>
        public DateTime? created { get; set; }

        /// <summary>
        /// Whether or not this room is archived.
        /// </summary>
        public bool? is_archived { get; set; }

        /// <summary>
        /// Whether or not this room is private.
        /// </summary>
        public bool? is_private { get; set; }

        /// <summary>
        /// User ID of the room owner.
        /// </summary>
        public int? owner_user_id { get; set; }

        /// <summary>
        /// Privacy setting (public/private).
        /// </summary>
        public string privacy { get; set; }

        /// <summary>
        /// Array containing user IDs and names of current room participants.
        /// </summary>
        public List<UserRaw> participants { get; set; }

        /// <summary>
        /// Whether or not to enable guest access for this room.  0 = false, 1 = true. (default: 0)
        /// </summary>
        public int? guest_access { get; set; }

        /// <summary>
        /// URL for guest access, if enabled.
        /// </summary>
        public string guest_access_url { get; set; }

        /// <summary>
        /// XMPP/Jabber ID of the room.
        /// </summary>
        public string xmpp_jid { get; set; }

        public Room ToRoom()
        {
            return new Room
            {
                RoomId = room_id,
                Name = name,
                Topic = topic,
                LastActive = last_active,
                Created = created,
                IsArchived = is_archived,
                IsPrivate = is_private,
                OwnerId = owner_user_id,
                AccessLevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), privacy ?? "private", true),
                Participants = participants.SelectSafely(u => u.ToUser()).ToList(),
                GuestAccessUrl = guest_access_url,
                XmppJabberId = xmpp_jid
            };
        }
    }

    internal class RoomsRaw : IEnumerable<RoomRaw>
    {
        public List<RoomRaw> rooms { get; set; }

        public IEnumerator<RoomRaw> GetEnumerator()
        {
            return rooms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
