using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security;

namespace VeOpenIdConnectClient
{
    public class ChallengeResult : IHttpActionResult
    {
        private string AuthenticationProvider { get; set; }
        private string RedirectUri { get; set; }
        private HttpRequestMessage RequestMessage { get; set; }

        public ChallengeResult(string provider, string redirectUri, HttpRequestMessage request)
        {
            AuthenticationProvider = provider;
            RedirectUri = redirectUri;
            RequestMessage = request;
        }

        /// <summary>
        /// De authentication challenge maakt van de http response een openid connect authentication response
        /// waarbij de status code 401 wordt gemaakt.
        /// Als deze response vervolgens langs de OWIN Middleware komt veranderd deze de statuscode naar 302 redirect.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Task with HttpResponseMessage</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var properties = new AuthenticationProperties() { RedirectUri = this.RedirectUri };
            RequestMessage.GetOwinContext().Authentication.Challenge(properties, AuthenticationProvider);

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = RequestMessage
            };

            return Task.FromResult(response);
        }
    }
}