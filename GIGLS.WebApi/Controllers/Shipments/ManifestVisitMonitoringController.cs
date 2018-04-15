using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    //[Authorize(Roles = "Shipment")]
    [RoutePrefix("api/manifestvisitmonitoring")]
    public class ManifestVisitMonitoringController : BaseWebApiController
    {
        private readonly IManifestVisitMonitoringService _service;

        public ManifestVisitMonitoringController(IManifestVisitMonitoringService service) : base(nameof(ManifestVisitMonitoringController))
        {
            _service = service;
        }

       // [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ManifestVisitMonitoringDTO>>> GetManifestVisitMonitorings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifests = await _service.GetManifestVisitMonitorings();

                return new ServiceResponse<IEnumerable<ManifestVisitMonitoringDTO>>
                {
                    Object = manifests
                };
            });
        }

       // [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{manifestVisitMonitoringId:int}")]
        public async Task<IServiceResponse<ManifestVisitMonitoringDTO>> GetManifestVisitMonitoringById(int manifestVisitMonitoringId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifests = await _service.GetManifestVisitMonitoringById(manifestVisitMonitoringId);

                return new ServiceResponse<ManifestVisitMonitoringDTO>
                {
                    Object = manifests
                };
            });
        }

       // [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<IEnumerable<ManifestVisitMonitoringDTO>>> GetManifestVisitMonitorings(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifests = await _service.GetManifestVisitMonitoringByWaybill(waybill);

                return new ServiceResponse<IEnumerable<ManifestVisitMonitoringDTO>>
                {
                    Object = manifests
                };
            });
        }
        
       // [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddManifest(ManifestVisitMonitoringDTO manifestVisitMonitoringDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifest = await _service.AddManifestVisitMonitoring(manifestVisitMonitoringDTO);
                return new ServiceResponse<object>
                {
                    Object = manifest
                };
            });
        }

      //  [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{manifestVisitMonitoringId:int}")]
        public async Task<IServiceResponse<bool>> DeleteManifestById(int manifestVisitMonitoringId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteManifestVisitMonitoring(manifestVisitMonitoringId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

      //  [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{manifestVisitMonitoringId:int}")]
        public async Task<IServiceResponse<bool>> UpdateManifestById(int manifestVisitMonitoringId, ManifestVisitMonitoringDTO manifestVisitMonitoringDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateManifestVisitMonitoring(manifestVisitMonitoringId, manifestVisitMonitoringDTO);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
