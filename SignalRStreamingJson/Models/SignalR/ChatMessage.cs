using System.ComponentModel.DataAnnotations;

namespace SignalRStreaming.BL.Models.SignalR
{
    public class ChatMessage
    {
        [Key]
        public int ChatID { get; set; }
        public string Username { get; set; }
        public DateTime MessageSent { get; set; }
        public string Message { get; set; }
        public string UserID { get; set; }
        public virtual AppUser Sender { get; set; }

        public ChatMessage(string message)
        {
            MessageSent = DateTime.Now;
            Message = message;
        }
    }
}
 