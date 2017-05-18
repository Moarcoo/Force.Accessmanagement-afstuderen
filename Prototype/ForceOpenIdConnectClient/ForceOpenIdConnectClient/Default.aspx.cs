using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForceOpenIdConnectClient.Helpers;
using ForceOpenIdConnectClient.Models;
using Microsoft.IdentityModel.Protocols;

namespace ForceOpenIdConnectClient
{
    public partial class Default : Page
    {
        protected IEnumerable<Claim> Claims { get; set; }
        protected UserRepresentation User { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated) OpenIdConnectHelpers.LoginWithOpenIdConnect(Context);
            Claims = Request.GetOwinContext().Authentication.User.Claims;
        }

        protected void CreateUser_Click(Object sender, EventArgs e)
        {
            // Create a user. I made a small Keycloak object User class based on the Keycloak documentation
            // This can be done the same way for all the Keycloak Administration API objects.
            // http://www.keycloak.org/docs-api/3.1/rest-api/index.html
            var userRepresentation = new UserRepresentation
            {
                username = "userMadeByForce",
                firstName = "Force",
                lastName = "Account",
                enabled = true
            };
            
            // Just like the CreateUserRepresentation call, every Administration API call needs an access token from a 
            // user with admin rights or use this client's service account and give it admin rights.
            var response = KeycloakHelpers.CreateUserRepresentation(userRepresentation, Context);
            if (response.IsSuccessStatusCode)
            {
                User = KeycloakHelpers.GetUserRepresentation(Context);
                
                // Here I add metadata to a user. this can for example be used when a contract is created. The contract id 
                // can then be stored in the user's metadata. 
                User.attributes.Add("contractId", new []{"12345"});
                var updatedUser = KeycloakHelpers.UpdateUserRepresentation(User, Context);
            }
        }
    }
}