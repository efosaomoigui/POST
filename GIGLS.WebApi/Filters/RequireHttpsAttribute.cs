using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;

namespace GIGLS.WebApi.Filters
{
    public class RequireHttpsAttribute: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            var request = actionContext.Request;
            
            if (request.RequestUri.Scheme == Uri.UriSchemeHttps)
            {
                var requestHtml = "<p>Request Requires HTTPS</p>";
                if (request.Method.Method == "GET")
                {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.Found);
                    actionContext.Response.Content = new StringContent(requestHtml, Encoding.UTF8, "text/html");

                    var urlbuider = new UriBuilder(request.RequestUri);
                    urlbuider.Scheme = Uri.UriSchemeHttps;
                    urlbuider.Port = 443;

                    actionContext.Response.Headers.Location = urlbuider.Uri;
                }
                else
                {
                    actionContext.Response = request.CreateResponse(HttpStatusCode.NotFound);
                    actionContext.Response.Content = new StringContent(requestHtml, Encoding.UTF8, "text/html");
                }
            }
        }
    }
}