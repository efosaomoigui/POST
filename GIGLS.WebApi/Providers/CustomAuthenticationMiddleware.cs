using POST.Core;
using POST.Core.Domain;
using POST.Core.Domain.Wallet;
using POST.Infrastructure.Persistence;
using Microsoft.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace POST.WebApi.Providers
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

            if (context.Response.StatusCode == 415 || context.Response.StatusCode == 400)
            {
                string contentType = String.Empty;
                var request = context.Request;
                foreach (var key in request.Headers.Keys)
                {
                    contentType += key + "=" + request.Headers[key] + "; ";
                }
                
                var remoteIp = request.RemoteIpAddress;
                var remotePort = request.RemotePort;
                var localIp = request.LocalIpAddress;
                var localPort = request.LocalPort;
                var ipport = $"Remote IP Address: {remoteIp}, Remote Port: {remotePort}, Local IP Address: {localIp}, Local Port: {localPort}";
                var log = new LogEntry() { Logger = contentType, MachineName = "CELLULANT", DateTime = DateTime.Now.ToString(), Level = context.Response.StatusCode.ToString(), CallSite = ipport};
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