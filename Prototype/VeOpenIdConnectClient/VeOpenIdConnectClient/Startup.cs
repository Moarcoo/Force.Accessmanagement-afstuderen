using System.Net;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using VeOpenIdConnectClient;

[assembly: OwinStartup(typeof(Startup))]

namespace VeOpenIdConnectClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // This line is only for developing and allows self signed certificates
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;

            ConfigureAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);   
        }
    }
}