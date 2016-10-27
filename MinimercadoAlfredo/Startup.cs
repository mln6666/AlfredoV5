using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MinimercadoAlfredo.Startup))]
namespace MinimercadoAlfredo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
