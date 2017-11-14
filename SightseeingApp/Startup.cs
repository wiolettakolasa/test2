using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SightseeingApp.Startup))]
namespace SightseeingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
