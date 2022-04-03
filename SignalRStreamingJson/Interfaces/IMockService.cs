using SignalRStreaming.BL.Models;
using SignalRStreamingJson.Models;
using SignalRStreamingJson.Models.SignalR;

namespace SignalRStreamingJson.Interfaces
{
    public interface IMockService
    {

        //POST METHODS
        Task<bool> PostUserAsync(User user);
        Task<bool> PostUserToFriendList(string userid, string addedUserid);

        //GET METHOD
        Task<User> GetUserAsync(string userid);
        Task<ChatFriends> GetMessagesAsync(int id);


        //UPDATES
        Task<bool> UpdateMessagesAsync(ChatFriends chatFriends);


        //STOCK TICKER METHODS

    }
}
