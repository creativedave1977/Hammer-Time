using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HammerTime.Startup))]
namespace HammerTime
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
