namespace SignalRStreaming.UI.Models.JWT
{
    public class AuthenticateResponse
    {
        public string? UserID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }

        public AuthenticateResponse(string id, string firstname, string lastname, string jwt, string refreshtoken)
        {
            UserID = id;
            Firstname = firstname;
            Lastname = lastname;
            JwtToken = jwt;
            RefreshToken = refreshtoken;
        }
    }
}
