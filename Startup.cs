using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PBIEmbeddedApp.Startup))]
namespace PBIEmbeddedApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
