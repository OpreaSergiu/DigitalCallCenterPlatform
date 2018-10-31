using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DigitalCallCenterPlatform.Startup))]
namespace DigitalCallCenterPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
