using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace VeOpenIdConnectClient.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Login()
        {
            var properties = new AuthenticationProperties() { RedirectUri = VirtualPathUtility.ToAbsolute(string.Format(CultureInfo.InvariantCulture, "~/")) };
            return new ChallengeResult(Constants.provider, properties.RedirectUri, this.Request);
        }

        [HttpGet]
        public IHttpActionResult Logout()
        {
            string[] authenticationTypes = { Constants.provider, DefaultAuthenticationTypes.ApplicationCookie };
            Request.GetOwinContext().Authentication.SignOut(authenticationTypes);
            return Ok();
        }

        [Authorize(Roles = "ve_access")]
        [HttpGet]
        public IHttpActionResult GetIdToken()
        {
            var aspClaims = Request.GetOwinContext().Authentication.User.Claims;

            var claims = from claim 
                         in aspClaims
                         select new
                         {
                             type = claim.Type,
                             value = claim.Value
                         };

            return Json(claims);
        }
    }
}