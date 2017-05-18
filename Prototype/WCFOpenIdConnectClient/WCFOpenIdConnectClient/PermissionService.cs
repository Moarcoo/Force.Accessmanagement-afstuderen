using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WCFOpenIdConnectClient
{
    public class PermissionService
    {
        private string AccessToken { get; set; }
        private string ResourceId { get; set; }

        public PermissionService(string accessToken, string resourceId)
        {
            AccessToken = accessToken;
            ResourceId = resourceId;
        }

        public async Task<string> CreatePermissionTicketToClient(string[] scopes) 
        {
            using (var client = new HttpClient())
            {
                // create permission ticket request with access token
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:8443/auth/realms/master/authz/protection/permission");
                request.Headers.Authorization = AuthenticationHeaderValue.Parse("bearer " + AccessToken);

                // specify the scopes and the resource id
                var requestMessage = JsonConvert.SerializeObject(new
                {
                    resource_set_id = ResourceId,
                    scopes = scopes
                });
                request.Content = new StringContent(requestMessage, Encoding.UTF8, "application/json");
                
                // request the permission ticket
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                var result = JObject.Parse(content);
                return (string) result["ticket"];
            }
        }

        public void EvaluateRelyingPartyToken()
        {
            throw new NotImplementedException();
        }
    }
}