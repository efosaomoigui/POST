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

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsIdentity claimsIdentity;
            var httpContext = HttpContext.Current;

            if (!(httpContext.User.Identity is ClaimsIdentity))
            {
                return false;
            }

            claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

            //var locIdClaims = claimsIdentity.FindFirst("LocationId");
            var ActivityClaims = claimsIdentity.FindAll("Activity");

            if (ActivityClaims ==null)
            {
                // just extra defense
                return false;
            }

            var activities = from a in ActivityClaims select a.Value;
            var activityList = (!String.IsNullOrEmpty(this.Activity)) ? this.Activity.Split(',') : new String[] { };

            if (activities.Intersect(activityList).Any() == false)
            {
                return false;
            }

            //Continue with the regular Authorize check
            return base.IsAuthorized(actionContext);
        }
    }
}