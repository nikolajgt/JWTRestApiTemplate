using SignalRStreamingJson.Models;

namespace SignalRStreamingJson.Interfaces
{
    public interface IMockRepository
    {


        //Post
        Task<bool> PostUserAsync(User user);

        //Get
        Task<User> GetUserAsync(string id);

        //Updates
        Task<bool> UpdateCustomerAsync(User user);

        //JWT Methods
        Task<User> Login(string username, string password);
        Task<User> TokenRefreshRevoke(string token);

    }
}
