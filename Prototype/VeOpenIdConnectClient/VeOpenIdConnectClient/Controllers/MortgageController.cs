using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Http;
using Microsoft.IdentityModel.Protocols;
using VeOpenIdConnectClient.TestService;

namespace VeOpenIdConnectClient.Controllers
{
    public class MortgageController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "ve_access")]
        public IHttpActionResult GetMortgageFileForUser(string dossierId)
        {
            // Get the access token
            var accessToken = GetSessionAccessToken(Request);
            if (accessToken == null) return null;

            // Add the access token to the servicecall's request headers.
            // This example is taken from: http://stackoverflow.com/questions/964433/how-to-add-a-custom-http-header-to-every-wcf-call
            // The post describes a better implementation but this one shall do for the prototype.
            var client = new TestServiceClient();
            MortgageFile mortgageFile;
            using (var scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = accessToken;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                mortgageFile = client.GetMortgageFileForUser(dossierId);
            }

            if (mortgageFile == null) return BadRequest();
            return Json(mortgageFile);
        }

        /// <summary>
        /// Returns the access token from the current session user
        /// </summary>
        /// <param name="request">Http request that contains the session</param>
        /// <returns>Access token jwt string</returns>
        private string GetSessionAccessToken(HttpRequestMessage request)
        {
            return request.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type == OpenIdConnectParameterNames.AccessToken)?.Value;
        }
    }
}