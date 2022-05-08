using Microsoft.AspNetCore.SignalR;
using SignalRStreamingJson.Interfaces;

namespace SignalRStreaming.BL.Hubs
{
    public class PublicChatHub : Hub
    {
        private readonly IMockService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        public PublicChatHub(IMockService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            await AddMessageToChat("", "User connected!");
            await base.OnConnectedAsync();
            var test = Context.GetHttpContext;
        }

        [HubMethodName("AddMessageToChat")]
        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("Ayooo", user, message);
        }
    }
}
