using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fanex.Security.Cryptography.Site.Startup))]
namespace Fanex.Security.Cryptography.Site
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
