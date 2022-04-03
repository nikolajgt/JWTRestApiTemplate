
using Microsoft.AspNetCore.SignalR;
using SignalRStreaming.BL.Models.SignalR;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models.SignalR;
using System.Security.Claims;

namespace SignalRStreamingJson.Hubs
{
    public class MockHub : Hub
    {
        private readonly IMockService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        public MockHub(IMockService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            await AddMessageToChat("", "User connected!");
            var response = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            await base.OnConnectedAsync();
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("Ayooo", user, message);
        }

        public async Task PrivateSendMessage(int id, string targetconnectionid, string username, string message)
        {
            await Clients.Client(targetconnectionid).SendAsync("ReceiveMessageToUser", username, targetconnectionid, message);

            var newModel = new ChatMessage(message);
            var response = await _service.GetMessagesAsync(id);
            response.ChatMessages.Add(newModel);
            await _service.UpdateMessagesAsync(response);
        }

        public async Task PrivateReciveMessage(string id, string username, string message)
        {
            await Clients.Caller.SendAsync("ReciveMessageToMe", username, message);

            var newModel = new ChatMessage(message);
        }

    }
}
