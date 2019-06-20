using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;
using GIGLS.Core.IServices.Business;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/hubmanifestwaybillmapping")]
    public class HUBManifestWaybillMappingController : BaseWebApiController
    {
        private readonly IHUBManifestWaybillMappingService _service;
        private readonly IScanService _scan;
        public HUBManifestWaybillMappingController(IHUBManifestWaybillMappingService service, IScanService scan)
            : base(nameof(HUBManifestWaybillMappingController))
        {
            _service = service;
            _scan = scan;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetAllHUBManifestWaybillMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetAllHUBManifestWaybillMappings();

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("manifestbydate")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetAllHUBManifestWaybillMappings(DateFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetAllHUBManifestWaybillMappings(dateFilterCriteria);

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybills(HUBManifestWaybillMappingDTO data)
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultipleHubManifest")]
        public async Task<IServiceResponse<bool>> MappingHUBManifestToWaybills(HUBManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingHUBManifestToWaybills(data.ManifestCode, data.Waybills,
                    data.DepartureServiceCentreId, data.DestinationServiceCentreId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiplemobile")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybillsMobile(HUBManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToWaybillsMobile(data.ManifestCode, data.Waybills);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifest/{manifest}")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetWaybillsInManifest(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _service.GetWaybillsInManifest(manifest);

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifestfordispatch")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetWaybillsInManifestForDispatch()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _service.GetWaybillsInManifestForDispatch();

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifestforwaybill/{waybill}")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetManifestForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetManifestForWaybill(waybill);

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = manifestDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("activemanifestforwaybill/{waybill}")]
        public async Task<IServiceResponse<HUBManifestWaybillMappingDTO>> GetActiveManifestForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetActiveManifestForWaybill(waybill);

                return new ServiceResponse<HUBManifestWaybillMappingDTO>
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
        public async Task<IServiceResponse<bool>> ReturnWaybillsInManifest(HUBManifestWaybillMappingDTO data)
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

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("signoff/{manifestCode}")]
        public async Task<IServiceResponse<bool>> SignOffDeliveryManifest(string manifestCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _scan.ScanSignOffDeliveryManifest(manifestCode);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifestwaitingforsignoff")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetManifestWaitingForSignOff()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetManifestWaitingForSignOff();

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifesthistoryforwaybill/{waybill}")]
        public async Task<IServiceResponse<List<HUBManifestWaybillMappingDTO>>> GetManifestHistoryForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetManifestHistoryForWaybill(waybill);

                return new ServiceResponse<List<HUBManifestWaybillMappingDTO>>
                {
                    Object = manifestDTO
                };
            });
        }
    }
}
