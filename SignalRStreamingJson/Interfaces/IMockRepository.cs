using SignalRStreaming.BL.Models;
using SignalRStreamingJson.Models;

namespace SignalRStreamingJson.Interfaces
{
    public interface IMockRepository
    {


        //Post
        Task<bool> PostUserAsync(User user);
        Task<bool> AddFriendAsync(ChatFriends chatfriend);

        //Get
        Task<User> GetUserAsync(string id);
        Task<ChatFriends> GetChatFriendsAsync(int id);

        //Updates
        Task<bool> UpdateCustomerAsync(User user);
        Task<bool> UpdateMessagesAsync(ChatFriends chat);

        //JWT Methods
        Task<User> Login(string username, string password);
        Task<User> TokenRefreshRevoke(string token);

    }
}
