using POST.Core.IServices;
using POST.Core.IServices.Shipments;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POST.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/supermanifest")]
    public class SuperManifestController : BaseWebApiController
    {
        private readonly ISuperManifestService _service;

        public SuperManifestController(ISuperManifestService service) : base(nameof(SuperManifestController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpGet]
        [Route("generatesupermanifestcode")]
        public async Task<IServiceResponse<string>> GenerateSuperManifestCode()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var code = await _service.GenerateSuperManifestCode();

                return new ServiceResponse<string>
                {
                    Object = code
                };
            });
        }
    }
}