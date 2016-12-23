using Microsoft.Owin;
using Owin;
using WebTask;

[assembly: OwinStartup(typeof(Startup))]
namespace WebTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
