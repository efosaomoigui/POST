using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Infrastructure.Persistence;
using Microsoft.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.WebApi.Providers
{
    //This class handles all the OwinMiddleware responses, so the name should 
    //not just focus on invalid authentication
    public class CustomAuthenticationMiddleware : OwinMiddleware
    {
        private readonly GIGLSContext _context = new GIGLSContext();
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

            if (context.Response.StatusCode == 415)
            {
                var contentType = context.Request.Headers.GetValues("Content-Type").First();
                var log = new LogEntry() { Logger = contentType, MachineName = "CELLULANT", Username = DateTime.Now.ToString() };
                _context.LogEntry.Add(log);
                await _context.SaveChangesAsync();
            }
        }
    }

    public static class ServerGlobalVariables
    {
        //Your other properties...
        public const string OwinChallengeFlag = "X-Challenge";
    }
}