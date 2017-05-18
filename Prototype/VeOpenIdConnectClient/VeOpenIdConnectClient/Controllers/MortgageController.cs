using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VeOpenIdConnectClient.Helpers;
using VeOpenIdConnectClient.TestService;

namespace VeOpenIdConnectClient.Controllers
{
    public class MortgageController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "ve_access")]
        public IHttpActionResult GetContract(string contractId)
        {
            var contract = GetContractById(contractId);
            return Json(contract);
        }

        private Contract GetContractById(string contractId)
        {
            // Get the access token
            var accessToken = OpenIdConnectHelpers.GetSessionAccessToken(Request);
            if (accessToken == null) return null;

            // Add the access token to the servicecall's request headers.
            // This example is taken from: http://stackoverflow.com/questions/964433/how-to-add-a-custom-http-header-to-every-wcf-call
            // The post describes a better implementation but this one shall do for the prototype.
            var client = new TestServiceClient();
            Contract contract = null;

            // Open a OperationContextScope so we can edit the service calls http headers
            using (var scope = new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = accessToken;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                try
                {
                    var rpt = GetRequestingPartyToken(accessToken);
                    contract = client.GetContract(contractId, rpt);
                }
                catch (FaultException<ServiceFault> fault)
                {
                    
                }
            }
            return contract;
        }

        private string GetRequestingPartyToken(string accessToken)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, Constants.keycloak_entitlement_endpoint
                    + "/" + Constants.wcfservice_clientid);
                request.Headers.Authorization = AuthenticationHeaderValue.Parse("bearer " + accessToken);
                var response = client.SendAsync(request).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JObject.Parse(content);
                var rpt = (string)result["rpt"];
                return rpt;
            }
        }
    }
}