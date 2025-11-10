using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oqtane.Components;
using Oqtane.Infrastructure;
using SignalR.Module.Chat.Hubs;
using SignalR.Module.Chat.Repository;
using SignalR.Module.Chat.Services;
using System.Linq;


namespace SignalR.Module.Chat.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
            });

        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IChatService, ServerChatService>();
            services.AddDbContextFactory<ChatContext>(opt => { }, ServiceLifetime.Transient);
            
            services.AddSignalR();
            
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    ["application/octet-stream"]);
            });        
        }
    }
}
