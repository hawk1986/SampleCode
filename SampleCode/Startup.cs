using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SampleCode.Startup))]
namespace SampleCode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}