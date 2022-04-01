using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models;

namespace SignalRStreamingJson.Services
{
    public class MockService : IMockService
    {

        private readonly IMockRepository _repository;

        public MockService(IMockRepository repository)
        {
            _repository = repository;
        }


        //POST METHODS
        public async Task<bool> PostUserAsync(User user)
        {
            var response = _repository.PostUserAsync(user);

            if (response == null)
                return false;

            return true;
        }

        

        //GET METHODS

        public async Task<User> GetUserAsync(string userid)
        {
            return await _repository.GetUserAsync(userid);
        }
    }
}
