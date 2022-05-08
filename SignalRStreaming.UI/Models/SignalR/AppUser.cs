using Microsoft.AspNetCore.Identity;

namespace SignalRStreaming.UI.Models.SignalR
{
    public class AppUser : IdentityUser
    {

        public AppUser()
        {
            Messages = new HashSet<ChatMessage>();
        }

        public virtual ICollection<ChatMessage> Messages { get; set; }
    }
}
