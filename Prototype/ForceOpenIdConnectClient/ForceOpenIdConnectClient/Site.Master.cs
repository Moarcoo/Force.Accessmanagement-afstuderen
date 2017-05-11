using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using OpenIdConnectClient;

namespace ForceOpenIdConnectClient
{
    public partial class Mainpage : System.Web.UI.MasterPage
    {
        protected string UserName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                if (!Context.User.IsInRole("force_access"))
                {
                    string[] authenticationTypes = { "OpenIdConnect", DefaultAuthenticationTypes.ApplicationCookie };
                    Context.GetOwinContext().Authentication.SignOut(authenticationTypes);
                }
                UserName = IdentityHelper.GetUserClaim(Request, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
            }
        }

        protected void Logout(object sender, LoginCancelEventArgs e)
        {
            string[] authenticationTypes = { "OpenIdConnect", DefaultAuthenticationTypes.ApplicationCookie};
            Context.GetOwinContext().Authentication.SignOut(authenticationTypes);
        }
    }
}