using Microsoft.IdentityModel.Tokens;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models;
using SignalRStreamingJson.Models.JWT;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SignalRStreamingJson.Services
{
    public class JWTService : IJWTService
    {
        private readonly string key;
        private readonly IMockRepository _repository;
        private static RNGCryptoServiceProvider? _cryptoService = new RNGCryptoServiceProvider();
        public JWTService(IMockRepository repository, IConfiguration config)
        {
            _repository = repository;
            key = config.GetSection("JWTkey").ToString();
        }

        public async Task<AuthenticateResponse> Authenticate(User c, string ipaddress)
        {
            var response = await _repository.Login(c.Username, c.Password);

            if (response == null)
                return null;

            var jwtToken = GenerateJwtToken(response);
            var refreshToken = GenerateRefreshToken(ipaddress);
            response.RefreshTokens.Add(refreshToken);

            await _repository.UpdateCustomerAsync(response);

            return new AuthenticateResponse(response, jwtToken, refreshToken.Token);
        }


        public async Task<AuthenticateResponse> RefreshToken(string token, string ipaddress)
        {
            var response = await _repository.TokenRefreshRevoke(token);

            if (response == null)
                return null;

            var refreshToken = response.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if (!refreshToken.IsActive)
                return null;

            var newRefreshToken = GenerateRefreshToken(ipaddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipaddress;
            refreshToken.ReplaceByToken = newRefreshToken.Token;

            response.RefreshTokens.Add(refreshToken);

            await _repository.UpdateCustomerAsync(response);
            var jwt = GenerateJwtToken(response);

            return new AuthenticateResponse(response, jwt, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipadress)
        {
            var response = await _repository.TokenRefreshRevoke(token);
            if (response == null)
                return false;

            var refreshToken = response.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if (!refreshToken.IsActive)
                return false;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipadress;

            response.RefreshTokens?.Add(refreshToken);

            await _repository.UpdateCustomerAsync(response);
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Encoding.ASCII.GetBytes(key);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, user.Roles.ToString()),
                    new Claim(ClaimTypes.Name, user.UserID),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username)

                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipaddress)
        {
            using (var service = _cryptoService)
            {
                var randomBytes = new byte[64];
                service.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddHours(24),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipaddress,
                };
            }
        }

        public async Task<User> GetCustomerByID(string id)
        {
            return await _repository.GetUserAsyncByID(id);
        }
    }
}
