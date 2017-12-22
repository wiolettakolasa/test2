using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.Configuration;
using System.Web.Services.Description;
using Microsoft.Extensions.DependencyInjection;
using SightseeingApp.Service;

[assembly: OwinStartupAttribute(typeof(SightseeingApp.Startup))]
namespace SightseeingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");
          
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<GeoCoordsService>();
            services.AddTransient<AlghService>();
        }
    }
}
