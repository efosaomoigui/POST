using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GIGLS.WebApi.Filters
{
    public class GIGLSRoleAuthorizeAttribute : AuthorizeAttribute 
    {
        //Authorize attribute properties
        //public string Role { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsIdentity claimsIdentity;
            var httpContext = HttpContext.Current;

            if (!(httpContext.User.Identity is ClaimsIdentity))
            {
                return false;
            }

            claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

            var RoleClaims = claimsIdentity.FindAll("Role"); //Roles from Identity

            if (RoleClaims == null)
            {
                // just extra defense
                return false;
            }

            var roles = from b in RoleClaims select b.Value;  //Admin from your login
            var roleList = (!String.IsNullOrEmpty(this.Roles)) ? this.Roles.Split(',') : new String[] { };

            if (roles.Intersect(roleList).Any() ==false)
            {
                return false;
            }

            //Continue with the regular Authorize check
            //return base.IsAuthorized(actionContext);
            return true;
        }
    }
}