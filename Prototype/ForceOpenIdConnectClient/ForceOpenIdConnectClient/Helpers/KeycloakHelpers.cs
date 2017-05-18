using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ForceOpenIdConnectClient.Models;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;

namespace ForceOpenIdConnectClient.Helpers
{
    public static class KeycloakHelpers
    {
        /// <summary>
        /// Create a user from Keycloak
        /// </summary>
        /// <param name="userRepresentation"></param>
        /// <param name="context"></param>
        /// <returns>The response message from Keycloak</returns>
        public static HttpResponseMessage CreateUserRepresentation(UserRepresentation userRepresentation, HttpContext context)
        {
            using (var client = new HttpClient())
            {
                // Choose between using the default admin account or the session user (The session user needs to have the appropriate manage-users role from the master-realm client).
                //var tokenResult = await GetAdminAccessToken();
                var accessToken = GetSessionAccessToken(context);
                var request = CreateHttpRequestMessage(HttpMethod.Post, Constants.keycloak_api_user_endpoint, accessToken);
                    
                var content = JsonConvert.SerializeObject(userRepresentation);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = client.SendAsync(request);
                return response.Result;
            }
        }

        /// <summary>
        /// Get a user from Keycloak
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A UserRepresentation of a Keycloak user</returns>
        public static UserRepresentation GetUserRepresentation(HttpContext context)
        {
            using (var client = new HttpClient())
            {
                var accessToken = GetSessionAccessToken(context);
                var request = CreateHttpRequestMessage(HttpMethod.Get,
                    Constants.keycloak_api_user_endpoint + "/" +
                    context.Request.GetOwinContext().Authentication.User.GetUserId(), accessToken);

                var response = client.SendAsync(request);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<UserRepresentation>(result);
                return user;
            }
        }

        /// <summary>
        /// Update a user in Keycloak
        /// </summary>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <returns>The response message from Keycloak</returns>
        public static async Task<HttpResponseMessage> UpdateUserRepresentation(UserRepresentation user, HttpContext context)
        {
            using (var client = new HttpClient())
            {
                var accessToken = GetSessionAccessToken(context);
                var request = CreateHttpRequestMessage(HttpMethod.Put,
                    Constants.keycloak_api_user_endpoint + "/" +
                    context.Request.GetOwinContext().Authentication.User.GetUserId(), accessToken);
                
                var content = JsonConvert.SerializeObject(user);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                return response;
            }
        }

        /// <summary>
        /// Returns the admin accesstoken
        /// </summary>
        /// <returns>TokenResponse which contains the access token</returns>
        private static string GetAdminAccessToken()
        {
            var tokenResponseTask = new TokenClient("http://localhost:8080/auth/realms/master/protocol/openid-connect/token").RequestAsync(new Dictionary<string, string>
            {
                [OpenIdConnectParameterNames.ClientId] = "admin-cli",
                [OpenIdConnectParameterNames.Username] = "admin",
                [OpenIdConnectParameterNames.Password] = "admin",
                [OpenIdConnectParameterNames.GrantType] = "password"
            });

            var accessToken = tokenResponseTask.Result.AccessToken;
            return accessToken;
        }

        /// <summary>
        /// Creates a HttpRequestMessage with the given parameters
        /// </summary>
        /// <param name="method">HttpMethod</param>
        /// <param name="url">The address of the request</param>
        /// <param name="token">Access token</param>
        /// <returns>HttpRequestMessage</returns>
        private static HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url, string token)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Authorization = AuthenticationHeaderValue.Parse("bearer " + token);
            return request;
        }

        /// <summary>
        /// Returns the access token from the current session user
        /// </summary>
        /// <param name="context">Http context that contains the session</param>
        /// <returns>Access token jwt string</returns>
        private static string GetSessionAccessToken(HttpContext context)
        {
            return context.Request.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type == OpenIdConnectParameterNames.AccessToken)?.Value;
        }
    }
}