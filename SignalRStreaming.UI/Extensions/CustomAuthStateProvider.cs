using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace SignalRStreaming.UI.Extensions
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private readonly IJSRuntime _jsRunetime;

        public CustomAuthStateProvider(IJSRuntime jSRuntime)
        {
            _jsRunetime = jSRuntime;
        }


        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await GetTokenAsync();
            var identity = new ClaimsIdentity();
            var anonymousState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            if (!string.IsNullOrEmpty(token))
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            var jwtExpire = identity.Claims.FirstOrDefault(c => c.Type == "exp");
            if (jwtExpire != null)
            {
                var datetime = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(jwtExpire.Value));
                if (datetime.UtcDateTime <= DateTime.UtcNow)
                    return anonymousState;
            }


            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

       public async Task<string> GetTokenAsync()
            => await _jsRunetime.InvokeAsync<string>("localStorage.getItem", "Token");


        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split(".")[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


    }
}
