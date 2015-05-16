using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleApplication.Web.Startup))]
namespace SampleApplication.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
