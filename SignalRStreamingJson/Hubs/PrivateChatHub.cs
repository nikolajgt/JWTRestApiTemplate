
using Microsoft.AspNetCore.SignalR;
using SignalRStreaming.BL.Models.SignalR;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models.SignalR;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using SignalRStreamingJson.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SignalRStreaming.BL.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrivateChatHub : Hub
    {
        private readonly IMockService _service;
        //private readonly AppUser _appUser;
        public static ConcurrentDictionary<string, User> MyUsers = new ConcurrentDictionary<string, User>();

        public PrivateChatHub(IMockService service)
        {
            _service = service;

            //_appUser = appUser;
        }

        //Overrided hub method that adds user to "online" dicionary
        //And send user data out for testing
        public override async Task OnConnectedAsync()
        {
            
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            User garbage;
            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            await base.OnDisconnectedAsync(exception);
        }

        //When user have Connected, it immedialtly sends a request to get
        //User object. That way its always updated when logged in.
        //It also adds user to ConcurrentDicionary, so we can see whos online
        public async Task GetUserInformation(string userid)
        {
            var user = await _service.GetUserAsync(userid);
            user.ConnectionID = Context.ConnectionId;
            MyUsers.TryAdd(user.Username, user);

            var userObjectJson = JsonConvert.SerializeObject(user);
            await Clients.Client(Context.ConnectionId).SendAsync("ClientReciveUser", userObjectJson);
        }

        //Gets a username, message and targetUsername. Makes a message object
        //And Serialized it. Then we search for username in MyUsers ( Dicionary )
        //And sends its to the right target.
        public async Task SendPrivateMessage(string username, string message, string targetUsername)
        {
            var newMessageModel = new ChatMessage(username, message);
            var NewMessageJson = JsonConvert.SerializeObject(newMessageModel);

            var targetconnectionid = MyUsers.FirstOrDefault(x => x.Key == targetUsername).Value;
            if(targetconnectionid != null)
                await Clients.Client(targetconnectionid.ConnectionID).SendAsync("ClientReciveMessage", NewMessageJson);
        }

        public async Task AddPrivatFriend(string username, string targetusername)
        {

        }



    }
}
