using SignalRStreaming.BL.Models.SignalR;
using SignalRStreamingJson.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SignalRStreaming.UI.Models
{
    public class ChatFriends
    {
        public ChatFriends() { }


        public int ID { get; set; }

        public User User { get; set; }

        public User UserAdded { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }

        public ChatFriends(User user, User useradded, List<ChatMessage> chatmessages)
        {
            User = user;
            UserAdded = useradded;
            ChatMessages = chatmessages;
        }
    }
}
