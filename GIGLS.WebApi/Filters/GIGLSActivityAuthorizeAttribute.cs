using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GIGLS.WebApi.Filters
{
    public class GIGLSActivityAuthorizeAttribute : AuthorizeAttribute
    {

        public string Activity { get; set; }
        //public string Roles { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsIdentity claimsIdentity;
            var httpContext = HttpContext.Current;

            Type tp = actionContext.ControllerContext.Controller.GetType();
            var dnAttribute = tp.GetCustomAttributes(typeof(AuthorizeAttribute), true).
                FirstOrDefault() as AuthorizeAttribute;
            if (dnAttribute != null)
            {
                Roles = dnAttribute.Roles;
            }

            if (!(httpContext.User.Identity is ClaimsIdentity))
            {
                return false;
            }

            claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

            //Roles from Identity
            var RoleClaims = claimsIdentity.FindAll("Role");
            if (RoleClaims == null)
            {
                // just extra defense
                return false;
            }
            var roles = from b in RoleClaims select b.Value;  //Admin from your login
            var roleList = (!String.IsNullOrEmpty(this.Roles)) ? this.Roles.Split(',') : new String[] { };

            //var locIdClaims = claimsIdentity.FindFirst("LocationId");
            var ActivityClaims = claimsIdentity.FindAll("Activity");

            if (ActivityClaims == null)
            {
                // just extra defense
                return false;
            }

            var activities = from a in ActivityClaims select a.Value;
            var activityList = (!String.IsNullOrEmpty(this.Activity)) ? this.Activity.Split(',') : new String[] { };

            var roleactivityList = new List<String> { };
            foreach (var roleItem in roleList)
            {
                foreach (var activityItem in activityList)
                {
                    roleactivityList.Add($"{activityItem.Trim()}.{roleItem.Trim()}");
                }
            }

            if (activities.Intersect(roleactivityList).Any() == false)
            {
                return false;
            }

            //Continue with the regular Authorize check
            var rst = base.IsAuthorized(actionContext);
            return rst;
        }
    }
}