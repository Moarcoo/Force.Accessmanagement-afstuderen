﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = IdentityModel.Jwk.JsonWebKey;

namespace WCFOpenIdConnectClient
{
    public static class OpenIdConnectHelpers
    {
        /// <summary>
        /// Gets claims from the OIDC UserInfo endpoint with the access token.
        /// </summary>
        /// <returns>ClaimsPrincipal with claims</returns>
        public static ClaimsPrincipal GetClaimsPrincipalWithToken()
        {
            // Extract the access token from the authorization header
            var accessTokenJwt = WebOperationContext.Current?.IncomingRequest.Headers[HttpRequestHeader.Authorization];
            if (accessTokenJwt == null) throw new FaultException<ServiceFault>(new ServiceFault("No access token in request"));

            // Validate the access token
            var token = ValidateToken(accessTokenJwt).Result;
            if (token == null) throw new FaultException<ServiceFault>(new ServiceFault("Token is not valid"));

            // Get the claims from the UserInfo endpoint
            var userInfoClient = new UserInfoClient(Constants.oidc_userinfo_endpoint);
            var userInfo = userInfoClient.GetAsync(accessTokenJwt).Result;
            var claims = userInfo.Claims;

            // Create and return a ClaimsPrincipal with the received claims
            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims, null, Constants.keycloak_name_claim, Constants.keycloak_service_roles_claim));

            return principal;
        }

        /// <summary>
        /// Validates a token.
        /// </summary>
        /// <param name="jwtToken">Json Web Token</param>
        /// <returns>a boolean indicating if the validation succeeded or failed</returns>
        public static async Task<ClaimsPrincipal> ValidateToken(string jwtToken)
        {
            ClaimsPrincipal token;
            // Check is token is well formed following the rfc7519 standard
            var tokenvalidator = new JwtSecurityTokenHandler();
            if (!tokenvalidator.CanReadToken(jwtToken)) return null;

            // Create a security key
            var rsa = await CreateRsaCryptoServiceProvider();

            // Validate the token
            try
            {
                SecurityToken securityToken;
                token = tokenvalidator.ValidateToken(
                    jwtToken,
                    new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Constants.keycloak_authority,
                        ValidateAudience = true,
                        ValidAudiences = new List<string>() { Constants.angularclient_aud_claim },
                        ValidateLifetime = true,
                        RoleClaimType = Constants.keycloak_service_roles_claim,
                        NameClaimType = Constants.keycloak_name_claim,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa)
                    },
                    out securityToken
                    );
            }
            catch (Exception e)
            {
                throw new FaultException<ServiceFault>(new ServiceFault("Something went wrong with the token validation:" + e.Message));
            }
            return token;
        }

        /// <summary>
        /// Creates a RSA security token based.
        /// </summary>
        /// <returns>A RSA encyrption provider</returns>
        private static async Task<RSACryptoServiceProvider> CreateRsaCryptoServiceProvider()
        {
            var keys = await GetTokenKeys();
            
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

        /// <summary>
        /// Login to Keycloak with the service account to get an access token for this client
        /// </summary>
        /// <returns>Access token for this client</returns>
        public static string AuthenticateClient()
        {
            var discoveryClient = new DiscoveryClient(Constants.keycloak_authority);
            var doc = discoveryClient.GetAsync();

            var tokenResponseTask = new TokenClient(doc.Result.TokenEndpoint).RequestAsync(new Dictionary<string, string>
            {
                [OpenIdConnectParameterNames.ClientId] = Constants.wcfservice_clientid,
                [OpenIdConnectParameterNames.ClientSecret] = Constants.wcfservice_secret,
                [OpenIdConnectParameterNames.GrantType] = "client_credentials",
                [OpenIdConnectParameterNames.ResponseType] = "token",
            });

            var tokenResponse = tokenResponseTask;
            return tokenResponse.Result.AccessToken;
        }
    }
}