
using Microsoft.AspNetCore.SignalR;
using SignalRStreaming.BL.Models.SignalR;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models.SignalR;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using SignalRStreamingJson.Models;

namespace SignalRStreamingJson.Hubs
{
    public class PrivateChatHub : Hub
    {
        private readonly IMockService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppUser _appUser;
        public static ConcurrentDictionary<string, User> MyUsers = new ConcurrentDictionary<string, User>();
        public PrivateChatHub(IMockService service, IHttpContextAccessor contextAccessor, AppUser appUser)
        {
            _service = service;
            _contextAccessor = contextAccessor;
            _appUser = appUser;
        }

        //Overrided hub method that adds user to "online" dicionary
        //And send user data out for testing
        public override async Task OnConnectedAsync()
        {
            MyUsers.TryAdd(Context.ConnectionId, new User() { ConnectionID = Context.ConnectionId });
            PushData();
            
            await base.OnConnectedAsync();
        }

        //Overrided hub method that removes "online" user
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            User garbage;

            MyUsers.TryRemove(Context.ConnectionId, out garbage);

            await base.OnDisconnectedAsync(ex);
        }

        public void PushData()
        {
            Clients.Clients(MyUsers.Keys.ToList()).SendAsync("GetConnectionID", MyUsers.Keys.First());
        }


        //Method for sending the message to the selected user
        public async Task PrivateSendMessage(int id, string targetconnectionid, string username, string message)
        {
           // await Clients.Client(targetconnectionid).SendAsync("ReceiveMessageToUser", username, targetconnectionid, message);
            await Clients.User(targetconnectionid).SendAsync("ReceiveMessageToUser", username, targetconnectionid, message);

            var newModel = new ChatMessage(message);
            var response = await _service.GetMessagesAsync(id);
            response.ChatMessages.Add(newModel);
            await _service.UpdateMessagesAsync(response);
        }

        //Method for displaying the message for the user

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
