using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Security;
using Newtonsoft.Json.Linq;

namespace WCFOpenIdConnectClient
{
    public class TestService : ITestService
    { 
        public string GetData(int value)
        {
            var claimsPrincipal = OpenIdConnectHelpers.GetClaimsPrincipalWithToken().Result;
                  
            if (claimsPrincipal.IsInRole("admin"))
            {
                var id = claimsPrincipal.GetUserId();
                var name = claimsPrincipal.Identity.Name;
                return $"Your userId is '{id}' and it was requested for {name}";
            }
            return "Not authorized";
        }

        public MortgageFile GetMortgageFileForUser(string dossierId)
        {
            var claimsPrincipal = OpenIdConnectHelpers.GetClaimsPrincipalWithToken().Result;
            if (!claimsPrincipal.IsInRole("service_access")) throw new UnauthorizedAccessException("user doesn't have rights to access this service"); // TODO: 401 returnen?

            /**
             * Use the claims in the ClaimsPrinciple for example to get
             * data from a database with the dossierId
             */
            if (dossierId != claimsPrincipal.GetClaimByName("dossierId"))
                throw new UnauthorizedAccessException("The given dossierId doesn't match the user's dossierId in Keycloak");

            byte[] b = new byte[16];
            new Random().NextBytes(b);

            return new MortgageFile()
            {
                Guid = new Guid(b),
                Bouwdepot = true,
                RentePercentage = 1.4,
                Whiteblabel = "TestWhitelabel"
            };
        }
    }
}
