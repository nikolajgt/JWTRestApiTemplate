using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRStreaming.BL.Hubs;
using SignalRStreaming.BL.Models;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models;
using SignalRStreamingJson.Models.SignalR;
using System;
using System.Collections.Concurrent;

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
            var response = await _repository.PostUserAsync(user);

            if (response == null)
                return false;

            return true;
        }

        public async Task<bool> PostUserToFriendList(string userid, string username)
        {
            var user = await _repository.GetUserAsyncByID(userid);
            var useradded = await _repository.GetUserAsyncByUsername(username);

            var newModel = new ChatFriends(user, useradded);
            return await _repository.AddFriendAsync(newModel);
        }

        

        //GET METHODS

        public async Task<User> GetUserAsync(string userid)
        {
            return await _repository.GetUserAsyncByID(userid);
        }

        public async Task<ChatFriends> GetMessagesAsync(int id)
        {
            return await _repository.GetChatFriendsAsync(id);
        }

        //UPDATES

        public async Task<bool> UpdateMessagesAsync(ChatFriends chatFriends)
        {
            return await _repository.UpdateMessagesAsync(chatFriends);
        }


        //STOCK TICKER METHODS

        private readonly static Lazy<MockService> _instance = new Lazy<MockService>(() => new MockService(GlobalHost.ConnectionManager.GetHubContext<StreamingMockHub>().Clients));

        private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        private readonly object _updateStockPricesLock = new object();

        //stock can go up or down by a percentage of this factor on each change
        private readonly double _rangePercent = .002;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Random _updateOrNotRandom = new Random();

        private readonly Timer _timer;
        private volatile bool _updatingStockPrices = false;

        private MockService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;

            _stocks.Clear();
            var stocks = new List<Stock>
            {
                new Stock { Symbol = "MSFT", Price = 30.31m },
                new Stock { Symbol = "APPL", Price = 578.18m },
                new Stock { Symbol = "GOOG", Price = 570.30m }
            };
            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));

            _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);

        }

        public static MockService Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.Values;
        }

        private void UpdateStockPrices(object state)
        {
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;

                    foreach (var stock in _stocks.Values)
                    {
                        if (TryUpdateStockPrice(stock))
                        {
                            BroadcastStockPrice(stock);
                        }
                    }

                    _updatingStockPrices = false;
                }
            }
        }

        private bool TryUpdateStockPrice(Stock stock)
        {
            // Randomly choose whether to update this stock or not
            var r = _updateOrNotRandom.NextDouble();
            if (r > .1)
            {
                return false;
            }

            // Update the stock price by a random factor of the range percent
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > .51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }

        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.updateStockPrice(stock);
        }

    }
}
