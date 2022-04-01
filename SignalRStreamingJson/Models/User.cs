using SignalRStreamingJson.Models.JWT;
using System.ComponentModel.DataAnnotations;

namespace SignalRStreamingJson.Models
{
    public class User
    {
        public User() { }

        [Key]
        public string? UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public Roles Roles { get; set; }
        
        public List<RefreshToken>? RefreshTokens { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        //Creates new user
        public User(string username, string password, string firstname, string lastname, string email, Roles role)
        {
            UserID = Guid.NewGuid().ToString();
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Roles = role;
            RefreshTokens = new List<RefreshToken>();
        }

        //Transfer from DTO to User
        public User(string userid, string username, string password, string firstname, string lastname, string email, Roles role)
        {
            UserID = userid;
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Roles = role;
        }
    }
}
