using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;
using GIGLS.Core.IServices.Business;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/PreShipmentManifestMapping")]
    public class PreShipmentManifestMappingController : BaseWebApiController
    {
        private readonly IPreShipmentManifestMappingService _service;
        private readonly IScanService _scan;
        public PreShipmentManifestMappingController(IPreShipmentManifestMappingService service, IScanService scan)
            : base(nameof(PreShipmentManifestMappingController))
        {
            _service = service;
            _scan = scan;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<PreShipmentManifestMappingDTO>>> GetAllManifestWaybillMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestMappings = await _service.GetAllManifestWaybillMappings();

                return new ServiceResponse<List<PreShipmentManifestMappingDTO>>
                {
                    Object = manifestMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybills(PreShipmentManifestMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToWaybills(data);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifest/{manifest}")]
        public async Task<IServiceResponse<List<PreShipmentManifestMappingDTO>>> GetWaybillsInManifest(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybillNumbersInManifest = await _service.GetWaybillsInManifest(manifest);

                return new ServiceResponse<List<PreShipmentManifestMappingDTO>>
                {
                    Object = waybillNumbersInManifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifestforpickup")]
        public async Task<IServiceResponse<List<PreShipmentManifestMappingDTO>>> GetWaybillsInManifestForPickup()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybillNumbersInManifest = await _service.GetWaybillsInManifestForPickup();

                return new ServiceResponse<List<PreShipmentManifestMappingDTO>>
                {
                    Object = waybillNumbersInManifest
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifestforwaybill/{waybill}")]
        public async Task<IServiceResponse<PreShipmentManifestMappingDTO>> GetManifestForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetManifestForWaybill(waybill);

                return new ServiceResponse<PreShipmentManifestMappingDTO>
                {
                    Object = manifestDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("removewaybillfrommanifest/{manifest}/{waybill}")]
        public async Task<IServiceResponse<bool>> RemoveWaybillFromManifest(string manifest, string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.RemoveWaybillFromManifest(manifest, waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedwaybillforpickupmanifest")]
        public async Task<IServiceResponse<List<PreShipmentDTO>>> GetUnMappedWaybillsForPickupManifest()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var unMappedWaybill = await _service.GetUnMappedWaybillsForPickupManifest();

                return new ServiceResponse<List<PreShipmentDTO>>
                {
                    Object = unMappedWaybill
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("generateMaifestCode")]
        public async Task<IServiceResponse<string>> GenerateManifestCode()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestCode = await _service.GenerateManifestCode();

                return new ServiceResponse<string>
                {
                    Object = manifestCode
                };
            });
        }

    }
}
