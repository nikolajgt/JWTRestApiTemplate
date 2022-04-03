using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRStreamingJson.Models;
using SignalRStreamingJson.Models.SignalR;
using SignalRStreamingJson.Services;

namespace SignalRStreaming.BL.Hubs
{
    [HubName("stockticker")]
    public class StreamingMockHub : Hub
    {
        private readonly MockService _mockService;

        public StreamingMockHub() : this(MockService.Instance) { }

        public StreamingMockHub(MockService stockTicker)
        {
            _mockService = stockTicker;
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _mockService.GetAllStocks();
        }
    }
}
