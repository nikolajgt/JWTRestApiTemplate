namespace SignalRStreaming.UI.Models.DTO
{
    public class UserDTO
    {
        public string? UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public Roles Roles { get; set; }
    }
}
