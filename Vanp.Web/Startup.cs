using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(Vanp.Web.Startup))]
namespace Vanp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
