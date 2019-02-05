using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;

namespace GIGLS.WebApi.Filters.Configuration
{
    public static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string OwinContext = "MS_OwinContext";

        public static bool IsIPAllowed(this HttpRequestMessage request)
        {
            if (!request.GetRequestContext().IsLocal)
            {
                string clientIPAddress = request.GetClientIPAddress();

                var ipFiltering = ConfigurationManager.GetSection("ipFiltering") as IPFilteringSection;
                if (ipFiltering != null && ipFiltering.IPAdresses != null && ipFiltering.IPAdresses.Count > 0)
                {
                    if (ipFiltering.IPAdresses.Cast<IPAddressElement>().Any(ip => (clientIPAddress == ip.Address && !ip.Denied)))
                    {
                        return true;
                    }
                    return false;
                }
            }

            return true;
        }

        public static string GetClientIPAddress(this HttpRequestMessage request)
        {
            //web hosting
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if(ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            //self-hosting
            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessageProperty.Name];
                if(remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            //owin hosting
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic ctx = request.Properties[OwinContext];
                if(ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }

            return null;
        }
    }
}