namespace SignalRStreamingJson.Models.JWT
{
    public class AuthenticateResponse
    {
        public string? UserID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwt, string refreshtoken)
        {
            UserID = user.UserID;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            JwtToken = jwt;
            RefreshToken = refreshtoken;
        }
    }
}
