using SignalRStreamingJson.Models;

namespace SignalRStreamingJson.Interfaces
{
    public interface IMockService
    {

        //POST METHODS
        Task<bool> PostUserAsync(User user);


        //GET METHOD
        Task<User> GetUserAsync(string userid);
    }
}
