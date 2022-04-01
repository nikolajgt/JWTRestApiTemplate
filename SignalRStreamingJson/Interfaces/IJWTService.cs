using SignalRStreamingJson.Models;
using SignalRStreamingJson.Models.JWT;

namespace SignalRStreamingJson.Interfaces
{
    public interface IJWTService
    {
        //Gets refresh and jwt from user login
        Task<AuthenticateResponse> Authenticate(User user, string ipaddress);

        //Sets active token to not active and gets new refresh and jwt
        Task<AuthenticateResponse> RefreshToken(string token, string ipaddress);

        //Sets active token to not active
        Task<bool> RevokeToken(string token, string ipadress);
        Task<User> GetCustomerByID(string id);
    }
}
