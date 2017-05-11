using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForceOpenIdConnectClient.Helpers;
using Microsoft.Owin.Security;
using OpenIdConnectClient;

namespace ForceOpenIdConnectClient
{
    public partial class Werkvoorraad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated) OpenIdConnectHelpers.LoginWithOpenIdConnect(Context);
            if (!Context.User.IsInRole("werkvoorraad")) Response.Redirect("AccessDenied.aspx");
        }
    }
}