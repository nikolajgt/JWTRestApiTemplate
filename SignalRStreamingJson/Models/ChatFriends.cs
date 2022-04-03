using SignalRStreaming.BL.Models.SignalR;
using SignalRStreamingJson.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SignalRStreaming.BL.Models
{
    public class ChatFriends
    {
        public ChatFriends() { }

        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public User UserAdded { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }

        public ChatFriends(User user, User useradded)
        {
            User = user;
            UserAdded = useradded;
            ChatMessages = new List<ChatMessage>();
        }
    }
}
