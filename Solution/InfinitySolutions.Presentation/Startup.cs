using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InfinitySolutions.Presentation.Startup))]
namespace InfinitySolutions.Presentation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
