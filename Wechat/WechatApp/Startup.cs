using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WechatApp.Startup))]
namespace WechatApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
