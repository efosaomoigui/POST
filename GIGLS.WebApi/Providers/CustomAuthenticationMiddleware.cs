using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GIGLS.WebApi.Providers
{
    //This class handles all the OwinMiddleware responses, so the name should 
    //not just focus on invalid authentication
        public class CustomAuthenticationMiddleware : OwinMiddleware
    {
        public CustomAuthenticationMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == 400 && context.Response.Headers.ContainsKey("AuthorizationResponse"))
            {
                context.Response.Headers.Remove("AuthorizationResponse");
                context.Response.StatusCode = 401;
            }
        }
    }

    public static class ServerGlobalVariables
    {
        //Your other properties...
        public const string OwinChallengeFlag = "X-Challenge";
    }
}