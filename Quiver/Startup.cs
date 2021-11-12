using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Quiver.Startup))]
namespace Quiver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
