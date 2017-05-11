using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security.Notifications;
using JsonWebKey = IdentityModel.Jwk.JsonWebKey;

namespace VeOpenIdConnectClient.Helpers
{
    public static class OpenIdConnectHelpers
    {
        /// <summary>
        /// Backchannel request to get an access token from Keycloak in exchange for an authorization code
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>Task that contains a token response</returns>
        public static async Task<TokenResponse> RequestAccessTokenAsync(
            AuthorizationCodeReceivedNotification notification)
        {
            var discoveryClient = new DiscoveryClient(Constants.keycloak_authority);
            var doc = await discoveryClient.GetAsync();

            var tokenResponseTask = new TokenClient(doc.TokenEndpoint).RequestAsync(new Dictionary<string, string>
            {
                [OpenIdConnectParameterNames.ClientId] = notification.Options.ClientId,
                [OpenIdConnectParameterNames.ClientSecret] = notification.Options.ClientSecret,
                [OpenIdConnectParameterNames.Code] = notification.ProtocolMessage.Code,
                [OpenIdConnectParameterNames.GrantType] = "authorization_code",
                [OpenIdConnectParameterNames.ResponseType] = "token",
                [OpenIdConnectParameterNames.RedirectUri] = notification.Options.RedirectUri
            });

            var tokenResponse = await tokenResponseTask;
            return tokenResponse;
        }

        /// <summary>
        /// Returns the access token from the current session user
        /// </summary>
        /// <param name="request">Http request that contains the session</param>
        /// <returns>Access token jwt string</returns>
        public static string GetSessionAccessToken(HttpRequestMessage request)
        {
            return request.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type == OpenIdConnectParameterNames.AccessToken)?.Value;
        }

        /// <summary>
        /// Creates a RSA security token based on Keycloak's public key.
        /// </summary>
        /// <returns>A RSA encyrption provider</returns>
        public static RSACryptoServiceProvider CreateRsaCryptoServiceProvider()
        {
            var keys = GetTokenKeys().Result;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
              new RSAParameters()
              {
                  Modulus = FromBase64Url(keys[0].N),
                  Exponent = FromBase64Url(keys[0].E)
              });
            return rsa;
        }

        /// <summary>
        /// Receives the token keys from the OIDC cert endpoint
        /// </summary>
        /// <returns>List of JsonWebKey</returns>
        private static async Task<IList<JsonWebKey>> GetTokenKeys()
        {
            var discoveryClient = new DiscoveryClient(Constants.keycloak_authority);
            var doc = await discoveryClient.GetAsync();
            return doc.KeySet.Keys;
        }

        /* This method comes from http://stackoverflow.com/questions/34403823/verifying-jwt-signed-with-the-rs256-algorithm-using-public-key-in-c-sharp. 
         * It is used because apparently Convert.FromBase64String() can't handle all special characters
         */
        private static byte[] FromBase64Url(string base64Url)
        {
            string padded = base64Url.Length % 4 == 0
                ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
            string base64 = padded.Replace("_", "/")
                                    .Replace("-", "+");
            return Convert.FromBase64String(base64);
        }
    }
}