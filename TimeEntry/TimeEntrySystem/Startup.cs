using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimeEntrySystem.Startup))]
namespace TimeEntrySystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
