using Microsoft.EntityFrameworkCore;
using SignalRStreaming.BL.Models;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models;

namespace SignalRStreamingJson.Repositorys
{
    public class MockRepository : IMockRepository
    {
        private readonly MyDbContext _db;

        public MockRepository(MyDbContext db)
        {
            _db = db;
        }

        //Post

        public async Task<bool> PostUserAsync(User user)
        {
            try
            {
                await _db.Users.AddAsync(user);
                return (await _db.SaveChangesAsync()) > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddFriendAsync(ChatFriends chatfriend)
        {
            try
            {
                await _db.ChatFriends.AddAsync(chatfriend);
                return (await _db.SaveChangesAsync()) > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //Get
        public async Task<User> GetUserAsyncByID(string id)
        {
            var response = await _db.Users.FirstOrDefaultAsync(x => x.UserID == id);
            if (response == null)
                return null;

            return response;
        }

        public async Task<User> GetUserAsyncByUsername(string username)
        {
            var response = await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (response == null)
                return null;

            return response;
        }

        public async Task<ChatFriends> GetChatFriendsAsync(int id)
        {
            var response = await _db.ChatFriends.FirstOrDefaultAsync(x => x.ID == id);
            if (response == null)
                return null;

            return response;
        }


        //Updates
        public async Task<bool> UpdateCustomerAsync(User user)
        {
            try
            {
                _db.Users.Update(user);
                return (await _db.SaveChangesAsync()) > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMessagesAsync(ChatFriends chat)
        {
            try
            {
                _db.ChatFriends.Update(chat);
                return (await _db.SaveChangesAsync()) > 0;
            }
            catch(Exception e)
            {
                return false;
            }
        }


        //JWT Methods

        public async Task<User> Login(string username, string password)
        {
            try
            {
                var response = await _db.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
                if (response == null)
                    return null;

                return response;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<User> TokenRefreshRevoke(string token)
        {
            try
            {
                var response = await _db.Users.SingleOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == token));
                if (response == null)
                    return null;

                return response;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
