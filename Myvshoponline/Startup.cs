using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Myvshoponline.Startup))]
namespace Myvshoponline
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
