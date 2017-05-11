using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols;
using System.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using VeOpenIdConnectClient.Helpers;
using Owin;

[assembly: OwinStartup(typeof(VeOpenIdConnectClient.Startup))]

namespace VeOpenIdConnectClient
{
	public partial class Startup
	{
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
			
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
            {
                ClientId = Constants.angularclient_aud_claim,
                ClientSecret = Constants.angularclient_secret,
                Authority = Constants.keycloak_authority,
                RedirectUri = "http://localhost:3276/",
                PostLogoutRedirectUri = "http://localhost:3276/",
                ResponseType = "code id_token",
                Scope = "openid",
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Constants.keycloak_authority,
                    ValidateAudience = true,
                    ValidAudience = Constants.angularclient_aud_claim,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new RsaSecurityKey(OpenIdConnectHelpers.CreateRsaCryptoServiceProvider()),
                    RoleClaimType = Constants.keycloak_client_roles_claim,
                    NameClaimType = Constants.keycloak_name_claim,
                },
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    // Alle messages komen hier langs
                    MessageReceived = notification =>
                    {
                        return Task.FromResult(0);
                    },
                    // Tokens zijn binnengekomen
                    SecurityTokenReceived = notification =>
                    {
                        return Task.FromResult(0);
                    },
                    // Tokens zijn gevalideerd en verwerkt in de Owin.security authentication ticket
                    SecurityTokenValidated = notification =>
                    {
                        // bewaar de id token voor uit te loggen
                        var idToken = notification.ProtocolMessage.IdToken;
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(OpenIdConnectParameterNames.IdToken, idToken));

                        return Task.FromResult(0);
                    },
                    AuthorizationCodeReceived = async notification =>
                    {
                        var tokenResponse = await OpenIdConnectHelpers.RequestAccessTokenAsync(notification);

                        // Save de access token op als claim
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(
                                    OpenIdConnectParameterNames.AccessToken, tokenResponse.AccessToken
                                    ));
                    },
                    // Redirect options
                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var token = notification.OwinContext.Authentication.User?.FindFirst(OpenIdConnectParameterNames.IdToken);
                            if (token != null)
                            {
                                notification.ProtocolMessage.IdTokenHint = token.Value;
                            }
                        }
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}