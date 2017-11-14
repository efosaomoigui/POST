using GIGL.GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GIGLS.WebApi.Models
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(User user)
        {

            List<Claim> claims = new List<Claim>();

            //var daysInWork = (DateTime.Now.Date - user.JoinDate).TotalDays;
            var daysInWork = (DateTime.Now.Date - user.DateCreated).TotalDays;

            if (daysInWork > 90)
            {
                claims.Add(CreateClaim("FTE", "1"));

            }
            else
            {
                claims.Add(CreateClaim("FTE", "0"));
            }

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}