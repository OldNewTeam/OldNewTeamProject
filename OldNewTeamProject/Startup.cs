using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OldNewTeamProject.Startup))]
namespace OldNewTeamProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
