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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated) OpenIdConnectHelpers.LoginWithOpenIdConnect(Context);
            Claims = Request.GetOwinContext().Authentication.User.Claims;
        }

        protected void CreateUser_Click(Object sender, EventArgs e)
        {
            var userRepresentation = new UserRepresentation
            {
                username = "userMadeByForce",
                firstName = "Force",
                lastName = "Account",
                enabled = true
            };
            //KeycloakHelpers.GetUserRepresentation(Context);
            var response = KeycloakHelpers.CreateUser(userRepresentation, Context);
        }
    }
}