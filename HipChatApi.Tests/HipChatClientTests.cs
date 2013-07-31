using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HipChatApi.Tests
{
    [TestFixture]
    public class HipChatClientTests
    {
        [Test]
        public void Debug()
        {
            var client = HipChatClient.Create("<YOUR HIPCHAT AUTH TOKEN HERE>");

            var rooms = client.GetRooms();

            Assert.That(rooms, Is.Not.Null);
            Assert.That(rooms.Count, Is.EqualTo(7));

            var users = client.GetUsers(false);

            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count, Is.EqualTo(7));
            Assert.That(users.Any(u => u.Email == "andrewa@2020research.com"));

            var messages = client.GetMessagesForRoom(246615);

            Assert.That(messages, Is.Not.Null);
            Assert.That(messages.Count, Is.EqualTo(22));

            foreach (var m in messages)
            {
                Console.WriteLine("{0} / {1}: {2}", m.SendingUser.Name, m.TimeSent, m.Text);
            }
        }
    }
}
