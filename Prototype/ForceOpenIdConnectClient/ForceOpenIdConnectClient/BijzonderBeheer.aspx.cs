using System;
using ForceOpenIdConnectClient.Helpers;

namespace ForceOpenIdConnectClient
{
    public partial class BijzonderBeheer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated) OpenIdConnectHelpers.LoginWithOpenIdConnect(Context);
            if (!Context.User.IsInRole("bijzonderbeheer")) Response.Redirect("AccessDenied.aspx");
        }
    }
}