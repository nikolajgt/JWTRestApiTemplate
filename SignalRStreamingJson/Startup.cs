using Microsoft.AspNetCore.Builder;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRStreamingJson.Startup))]
namespace SignalRStreamingJson
{
    public class Startup
    {
        public void Configure(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
