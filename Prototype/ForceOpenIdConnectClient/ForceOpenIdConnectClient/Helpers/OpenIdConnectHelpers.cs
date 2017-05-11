using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using JsonWebKey = IdentityModel.Jwk.JsonWebKey;

namespace ForceOpenIdConnectClient.Helpers
{
    public static class OpenIdConnectHelpers
    {
        /// <summary>
        /// Triggers the Owin OpenId Connect middleware for authentication
        /// </summary>
        /// <param name="context">Current Http context</param>
        public static void LoginWithOpenIdConnect(HttpContext context)
        {
            var properties = new AuthenticationProperties() { RedirectUri = context.Request.Url.ToString() };
            context.GetOwinContext().Authentication.Challenge(properties, Constants.provider);
        }

        /// <summary>
        /// Backchannel request to get an access token from Keycloak in exchange for an authorization code
        /// </summary>
        /// <param name="notification"></param>
        /// <returns>Task that contains a token response</returns>
        public static async Task<TokenResponse> RequestAccessTokenAsync(AuthorizationCodeReceivedNotification notification)
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
        /// Creates a RSA security token based.
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