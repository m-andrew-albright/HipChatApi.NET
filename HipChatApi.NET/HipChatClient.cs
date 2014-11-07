using System.Linq;
using HipChatApi.Models;
using RestSharp;
using System;
using System.Collections.Generic;

namespace HipChatApi
{
    public interface IHipChatClient
    {
        Room CreateRoom(Room room);
        Room CreateRoom(string name, int ownerId, AccessLevel? access = null, bool? allowGuests = null, string topic = null);

        bool DeleteRoom(int id);

        /// <summary>
        /// Retrieves messages for the given room.  All dates/times will be in UTC.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <param name="messagesDate">The date for which you wish to retrieve messages, in UTC.  
        /// If null, 75 most recent messages will be retrieved.</param>
        /// <returns>A list of the 75 most recent messages from the given room.</returns>
        List<Message> GetMessagesForRoom(int roomId, DateTime? messagesDate = null);

        List<Room> GetRooms();

        bool SendMessage(Message message);
        bool SendMessage(int roomId, string from, string text, MessageFormat? format = null, bool? notify = null, Color? backgroundColor = null);

        bool ChangeRoomTopic(int roomId, string topic, string from = null);

        Room GetRoom(int roomId);

        User CreateUser(User user);
        User CreateUser(string email, string name, string mentionName = null, string title = null, bool? isGroupAdmin = null, string password = null);

        bool DeleteUser(int id);

        List<User> GetUsers(bool? includeDeleted = null);

        User GetUser(int id);

        bool UndeleteUser(int id);

        User UpdateUser(User user);
        User UpdateUser(int userId, string email = null, string name = null, string mentionName = null, string title = null, bool? isGroupAdmin = null, string password = null);
    }

    public class HipChatClient : IHipChatClient
    {
        private static class Services
        {
            public const string Url = "v1/{service}";

            public static class Rooms
            {
                private const string Base = "rooms/";
                public const string  Create = Base + "create",
                    Delete = Base + "delete",
                    History = Base + "history",
                    List = Base + "list",
                    Message = Base + "message",
                    Topic = Base + "topic",
                    Show = Base + "show";
            }

            public static class Users
            {
                private const string Base = "users/";
                public const string Create = Base + "create",
                    Delete = Base + "delete",
                    List = Base + "list",
                    Show = Base + "show",
                    Undelete = Base + "undelete",
                    Update = Base + "update";

            }
        }

        private static class Params
        {
            public const string RoomId = "room_id",
                Date = "date",
                Topic = "topic",
                From = "from",
                UserId = "user_id",
                IncludeDeleted = "include_deleted";
        }

        private IRestClient RestClient { get; set; }

        private string AuthToken { get; set; }

        public HipChatClient(IRestClient restClient, string authToken)
        {
            restClient.BaseUrl = new Uri("https://api.hipchat.com");
            RestClient = restClient;
            AuthToken = authToken;
        }

        private RestRequest GetRequestBase(string service, Method method)
        {
            var request = new RestRequest(Services.Url, method);
            request.AddUrlSegment("service", service);
            request.AddParameter("auth_token", AuthToken);
            request.AddParameter("format", "json");
            return request;
        }

        private T Execute<T>(IRestRequest request) where T : new()
        {
            var response = RestClient.Execute<T>(request);
            return response.Success() ? response.Data : default(T);
        }

        public Room CreateRoom(Room room)
        {
            var request = GetRequestBase(Services.Rooms.Create, Method.POST);
            request.AddObject(room.ToRoomRaw());

            return Execute<RoomRaw>(request).ToRoom();
        }

        public Room CreateRoom(string name, int ownerId, AccessLevel? access = null, bool? allowGuests = null, string topic = null)
        {
            return CreateRoom(new Room
            {
                Name = name,
                OwnerId = ownerId,
                AccessLevel = access,
                AllowsGuests = allowGuests,
                Topic = topic
            });
        }

        public bool DeleteRoom(int id)
        {
            var request = GetRequestBase(Services.Rooms.Delete, Method.POST);
            request.AddParameter(Params.RoomId, id);

            return RestClient.Execute(request).Success();
        }

        public List<Message> GetMessagesForRoom(int roomId, DateTime? messagesDate = null)
        {
            var request = GetRequestBase(Services.Rooms.History, Method.GET);
            request.AddParameter(Params.RoomId, roomId);
            //recent

            request.AddParameter(Params.Date, messagesDate != null ? messagesDate.Value.ToString("yyyy-MM-dd") : "recent");
            

            return Execute<IncomingMessagesRaw>(request).SelectSafely(m => m.ToMessage()).ToList();
        }

        public List<Room> GetRooms()
        {
            return Execute<RoomsRaw>(GetRequestBase(Services.Rooms.List, Method.GET)).SelectSafely(r => r.ToRoom()).ToList();
        }

        public bool SendMessage(Message message)
        {
            var request = GetRequestBase(Services.Rooms.Message, Method.POST);

            request.AddObject(message.ToOutgoing());

            return RestClient.Execute(request).Success();
        }

        public bool SendMessage(int roomId, string from, string text, MessageFormat? format = null, bool? notify = null, Color? backgroundColor = null)
        {
            return SendMessage(new Message
            {
                RoomId = roomId.ToString(),
                From = from,
                Text = text,
                MessageFormat = format,
                Notify = (notify ?? false) ? 1 : 0,
                Color = backgroundColor
            });
        }

        public bool ChangeRoomTopic(int roomId, string topic, string from = null)
        {
            var request = GetRequestBase(Services.Rooms.Topic, Method.POST);

            request.AddParameter(Params.RoomId, roomId);
            request.AddParameter(Params.Topic, topic);

            if (from != null)
            {
                request.AddParameter(Params.From, from);
            }

            return RestClient.Execute(request).Success();
        }

        public Room GetRoom(int id)
        {
            var request = GetRequestBase(Services.Rooms.Show, Method.GET);

            request.AddParameter(Params.RoomId, id);

            return Execute<RoomRaw>(request).ToRoom();
        }

        public User CreateUser(User user)
        {
            var request = GetRequestBase(Services.Users.Create, Method.POST);

            request.AddObject(user.ToUserRaw());

            return Execute<UserRaw>(request).ToUser();
        }

        public User CreateUser(string email, string name, string mentionName = null, string title = null, bool? isGroupAdmin = null, string password = null)
        {
            return CreateUser(new User
            {
                Email = email,
                Name = name,
                MentionName = mentionName,
                Title = title,
                IsGroupAdmin = isGroupAdmin,
                Password = password
            });
        }

        public bool DeleteUser(int id)
        {
            var request = GetRequestBase(Services.Users.Create, Method.POST);

            request.AddParameter(Params.UserId, id);

            return RestClient.Execute(request).Success();
        }

        public List<User> GetUsers(bool? includeDeleted = null)
        {
            var request = GetRequestBase(Services.Users.List, Method.GET);

            if (includeDeleted != null)
            {
                request.AddParameter(Params.IncludeDeleted, includeDeleted);
            }

            return Execute<UsersRaw>(request).SelectSafely(u => u.ToUser()).ToList();
        }

        public User GetUser(int id)
        {
            var request = GetRequestBase(Services.Users.Show, Method.GET);

            request.AddParameter(Params.UserId, id);

            return Execute<UserRaw>(request).ToUser();
        }

        public bool UndeleteUser(int id)
        {
            var request = GetRequestBase(Services.Users.Undelete, Method.POST);

            request.AddParameter(Params.UserId, id);

            return RestClient.Execute<User>(request).Success();
        }

        public User UpdateUser(User user)
        {
            var request = GetRequestBase(Services.Users.Update, Method.POST);

            request.AddObject(user.ToUserRaw());

            return Execute<UserRaw>(request).ToUser();
        }

        public User UpdateUser(int userId, string email = null, string name = null, string mentionName = null, string title = null, bool? isGroupAdmin = null, string password = null)
        {
            return UpdateUser(new User
            {
                UserId = userId.ToString(),
                Email = email,
                Name = name,
                MentionName = mentionName,
                Title = title,
                IsGroupAdmin = isGroupAdmin,
                Password = password
            });
        }

        public static HipChatClient Create(string authToken)
        {
            return new HipChatClient(new RestClient(), authToken);
        }
    }
}
