using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/manifestwaybillmapping")]
    public class ManifestWaybillMappingController : BaseWebApiController
    {
        private readonly IManifestWaybillMappingService _service;
        public ManifestWaybillMappingController(IManifestWaybillMappingService service) : base(nameof(ManifestWaybillMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetAllManifestWaybillMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetAllManifestWaybillMappings();

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybills(ManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToWaybills(data.ManifestCode, data.Waybills);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifest/{manifest}")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetWaybillsInManifest(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _service.GetWaybillsInManifest(manifest);

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifestfordispatch")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetWaybillsInManifestForDispatch()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _service.GetWaybillsInManifestForDispatch();

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifestforwaybill/{waybill}")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetManifestForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetManifestForWaybill(waybill);

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = manifestDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("activemanifestforwaybill/{waybill}")]
        public async Task<IServiceResponse<ManifestWaybillMappingDTO>> GetActiveManifestForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetActiveManifestForWaybill(waybill);

                return new ServiceResponse<ManifestWaybillMappingDTO>
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

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("returnwaybillsinManifest")]
        public async Task<IServiceResponse<bool>> ReturnWaybillsInManifest(ManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.ReturnWaybillsInManifest(data.ManifestCode, data.Waybills);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedwaybillfordeliverymanifest")]
        public async Task<IServiceResponse<List<ShipmentDTO>>> GetUnmappedManifestForDeliveryServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var unMappedWaybill = await _service.GetUnMappedWaybillsForDeliveryManifestByServiceCentre();

                return new ServiceResponse<List<ShipmentDTO>>
                {
                    Object = unMappedWaybill
                };
            });
        }
    }
}
