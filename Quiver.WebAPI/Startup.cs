using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Collections.Generic;
using System.Net;

[assembly: OwinStartupAttribute(typeof(Quiver.WebAPI.Startup))]
namespace Quiver.WebAPI
{
    public partial class Startup
    {
        public static List<HubUserConnection> userConnectionList = new List<HubUserConnection>();

        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var config = new HubConfiguration()
                {
                    EnableJSONP = true
                };

                var idProvider = new CustomUserIdProvider();
                GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

                map.RunSignalR(config);
            });
            //var config = new HubConfiguration()
            //{
            //    EnableJSONP = true
            //};

            //var idProvider = new CustomUserIdProvider();
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            //app.MapSignalR(config);

            ConfigureAuth(app);
        }
    }
}
