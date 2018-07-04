using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SqlAzureBackend.Startup))]
namespace SqlAzureBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
