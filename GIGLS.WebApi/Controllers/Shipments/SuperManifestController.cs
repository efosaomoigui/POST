using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
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