using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Owin;

[assembly: OwinStartup(typeof(ForceOpenIdConnectClient.Startup))]

namespace ForceOpenIdConnectClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // This line is only for developing and allows self signed certificates
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;

            ConfigureAuth(app);
        }
    }
}
