using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdAuth.Startup))]
namespace AdAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}