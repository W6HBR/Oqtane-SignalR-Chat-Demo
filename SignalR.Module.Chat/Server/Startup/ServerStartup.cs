//using BlazorSignalRApp.Client.Pages;
//using BlazorSignalRApp.Components;
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


using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using static Microsoft.AspNetCore.Components.Web.RenderMode;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;



namespace SignalR.Module.Chat.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddSignalR();

            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    ["application/octet-stream"]);
            });

            //var app = builder.Build();
            app = builder.Build();

            app.UseResponseCompression();

            // Configure the HTTP request pipeline.
            /*
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            */
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //app.MapStaticAssets();
            app.UseAntiforgery();

            //app.MapRazorComponents<App>()
            //    .AddInteractiveWebAssemblyRenderMode();
           //     .AddAdditionalAssemblies(typeof(BlazorSignalRApp.Client._Imports).Assembly);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                //endpoints.MapControllers();
                //endpoints.MapFallback();
            });
            //app.MapHub<ChatHub>("/chathub");

        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IChatService, ServerChatService>();
            services.AddDbContextFactory<ChatContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
