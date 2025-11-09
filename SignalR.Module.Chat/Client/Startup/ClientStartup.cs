using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using SignalR.Module.Chat.Services;

namespace SignalR.Module.Chat.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IChatService)))
            {
                services.AddScoped<IChatService, ChatService>();
            }
        }
    }
}
