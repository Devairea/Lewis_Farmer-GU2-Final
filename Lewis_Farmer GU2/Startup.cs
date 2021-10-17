using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lewis_Farmer_GU2.Startup))]
namespace Lewis_Farmer_GU2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
