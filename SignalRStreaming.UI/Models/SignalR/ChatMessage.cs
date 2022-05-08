using System.ComponentModel.DataAnnotations;

namespace SignalRStreaming.UI.Models.SignalR
{
    public class ChatMessage
    {
        [Key]
        public int ChatID { get; set; }
        public string Username { get; set; }
        public DateTime MessageSent { get; set; }
        public string Message { get; set; }
      //  public virtual AppUser Sender { get; set; }

        public ChatMessage(string username, string message)
        {
            Username = username;
            MessageSent = DateTime.Now;
            Message = message;
        }

        public ChatMessage() { }
    }
}
 