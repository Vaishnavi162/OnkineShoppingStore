using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OnlineShopingStore.Startup))]
namespace OnlineShopingStore
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Example: Reading a connection string
            var connStr = Configuration.GetConnectionString("DefaultConnection");

            // Add services as needed
        }
    }
}
