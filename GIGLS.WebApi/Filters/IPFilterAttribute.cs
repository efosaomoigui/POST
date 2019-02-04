using GIGLS.WebApi.Filters.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GIGLS.WebApi.Filters
{
    public class IPFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return actionContext.Request.IsIPAllowed();
        }
    }
}