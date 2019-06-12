using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CEPA.CCO.UI.Web.HOST.Startup))]
namespace CEPA.CCO.UI.Web.HOST
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
