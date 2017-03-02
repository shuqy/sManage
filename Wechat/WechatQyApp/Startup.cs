using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WechatQyApp.Startup))]
namespace WechatQyApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
