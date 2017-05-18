using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Newtonsoft.Json.Linq;

namespace WCFOpenIdConnectClient
{
    public class TestService : ITestService
    { 
        public string GetData(int value)
        {
            var claimsPrincipal = OpenIdConnectHelpers.GetClaimsPrincipalWithToken();

            if (claimsPrincipal.IsInRole("admin"))
            {
                var id = claimsPrincipal.GetUserId();
                var name = claimsPrincipal.Identity.Name;
                return $"Your userId is '{id}' and it was requested for {name}";
            }
            else
            {
                throw new FaultException<ServiceFault>(new ServiceFault("Not authorized"));
            }
        }

        public Contract GetContract(string contractId, string rpt)
        {
            /*
             * Get claims from Keycloak with the access token.  
             */
            var claimsPrincipal = OpenIdConnectHelpers.GetClaimsPrincipalWithToken();
            if (!claimsPrincipal.IsInRole("service_access")) throw new FaultException<ServiceFault>(new ServiceFault("user doesn't have rights to access this service"));
            
            /*
             * Check the request for RPT
             */
            if (rpt == null) throw new FaultException<ServiceFault>(new ServiceFault("Request has no Relying Party Token"));

            /*
             * Validate and decode the Requesting Party Token
             * For actual usage of this RPT, it's best to write a service that maps the json permissions to
             * a better format.
             */
            var rptClaims = OpenIdConnectHelpers.ValidateToken(rpt).Result;
            if (rptClaims == null) throw new FaultException<ServiceFault>(new ServiceFault("Requesting Party Token is not valid"));

            // Get the contract with the given parameters from the database
            // The resource should contain a Keycloak resource id 
            // For example "2b3cbcdb-c8f8-4b38-ad65-5210f7d1e05b"

            // The next step is to search the RPT claim "authorization" for a resource that has the resource id and 
            // see if the user has the right CRUD scope for this API and base a deciscion on that

            return new Contract()
            {
                Guid = Guid.NewGuid(),
                Bouwdepot = true,
                RentePercentage = 1.4,
                Whitelabel = "TestWhitelabel",
                Permissions = rptClaims.GetClaimByName("authorization")
            };

            /*
            * Use the claims in the ClaimsPrinciple for example to get
            * data from a database with the dossierId
            */
            //            if (dossierId != claimsPrincipal.GetClaimByName("dossierId"))
            //                throw new FaultException<ServiceFault>(new ServiceFault("The given dossier id doesn't match the user's dossier id in Keycloak"));

        }
    }
}
