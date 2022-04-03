
using Microsoft.AspNetCore.SignalR;
using SignalRStreaming.BL.Models.SignalR;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models.SignalR;
using System.Security.Claims;
using Newtonsoft.Json;

namespace SignalRStreamingJson.Hubs
{
    public class PrivateChatHub : Hub
    {
        private readonly IMockService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        public PrivateChatHub(IMockService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
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
        }

        public async Task InitializeChat(int id, string targetconnectionid)
        {
            var response = await _service.GetMessagesAsync(id);
            var toJson = JsonConvert.SerializeObject(response);
            await Clients.Client(targetconnectionid).SendAsync("InitializeChat", toJson);
        }

    }
}
