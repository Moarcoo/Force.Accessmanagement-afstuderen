using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForceOpenIdConnectClient.Helpers;

namespace ForceOpenIdConnectClient
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated) OpenIdConnectHelpers.LoginWithOpenIdConnect(Context);
        }
    }
}