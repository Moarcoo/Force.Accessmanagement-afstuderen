using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace VeOpenIdConnectClient.Helpers
{
    public static class ClaimPrincipalExtensionMethods
    {
        public static string GetUserId(this ClaimsPrincipal cp)
        {
            var id = cp.Claims.FirstOrDefault(x => x.Type == Constants.id_claim)?.Value;
            if (id == null) throw new NullReferenceException("No id claim found");
            return id;
        }

        public static string GetClaimByName(this ClaimsPrincipal cp, string claimName)
        {
            var claimValue = cp.Claims.FirstOrDefault(x => x.Type == claimName)?.Value;
            if (claimValue == null) throw new NullReferenceException("No claim found with the given claim name");
            return claimValue;
        }
    }
}