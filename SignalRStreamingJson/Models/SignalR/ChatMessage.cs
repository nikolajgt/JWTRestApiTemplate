using System.ComponentModel.DataAnnotations;

namespace SignalRStreaming.BL.Models.SignalR
{
    public class ChatMessage
    {
        [Key]
        public int ChatID { get; set; }
        public DateTime MessageSent { get; set; }
        public string Message { get; set; }

        public ChatMessage(string message)
        {
            MessageSent = DateTime.Now;
            Message = message;
        }
    }
}
