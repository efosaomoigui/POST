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
    [RoutePrefix("api/manifestwaybillmapping")]
    public class ManifestWaybillMappingController : BaseWebApiController
    {
        private readonly IManifestWaybillMappingService _service;
        private readonly IScanService _scan;
        public ManifestWaybillMappingController(IManifestWaybillMappingService service, IScanService scan)
            : base(nameof(ManifestWaybillMappingController))
        {
            _service = service;
            _scan = scan;
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("manifestbydate")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetAllManifestWaybillMappings(DateFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetAllManifestWaybillMappings(dateFilterCriteria);

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("pickupmanifestbydate")]
        public async Task<IServiceResponse<List<PickupManifestWaybillMappingDTO>>> GetAllPickupManifestWaybillMappings(DateFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var pickupmanifestWayBillNumberMappings = await _service.GetAllPickupManifestWaybillMappings(dateFilterCriteria);

                return new ServiceResponse<List<PickupManifestWaybillMappingDTO>>
                {
                    Object = pickupmanifestWayBillNumberMappings
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiplemobile")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybillsMobile(ManifestWaybillMappingDTO data)
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiplepickup")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybillsPickup(PickupManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingManifestToWaybillsPickup(data.ManifestCode, data.Waybills);
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
        [Route("pickupwaybillsinmanifest/{manifest}")]
        public async Task<IServiceResponse<List<PickupManifestWaybillMappingDTO>>> GetWaybillsInPickupManifest(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybillNumbersIPickupManifest = await _service.GetWaybillsInPickupManifest(manifest);

                return new ServiceResponse<List<PickupManifestWaybillMappingDTO>>
                {
                    Object = waybillNumbersIPickupManifest
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("pickupmanifest/{manifest}")]
        public async Task<IServiceResponse<PickupManifestDTO>> GetPickupManifestDetails(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybillNumbersIPickupManifest = await _service.GetPickupManifest(manifest);

                return new ServiceResponse<PickupManifestDTO>
                {
                    Object = waybillNumbersIPickupManifest
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
        
        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("removewaybillfrompickupmanifest/{manifest}/{waybill}")]
        public async Task<IServiceResponse<bool>> RemoveWaybillFromPickupManifest(string manifest, string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.RemoveWaybillFromPickupManifest(manifest, waybill);
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedwaybillforpickupmanifest/{stationId:int}")]
        public async Task<IServiceResponse<List<PreShipmentMobileDTO>>> GetUnMappedWaybillsForPickupManifest(int stationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var unMappedWaybill = await _service.GetUnMappedWaybillsForPickupManifest(stationId);

                return new ServiceResponse<List<PreShipmentMobileDTO>>
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
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetManifestWaitingForSignOff()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestGroupWayBillNumberMappings = await _service.GetManifestWaitingForSignOff();

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = manifestGroupWayBillNumberMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifesthistoryforwaybill/{waybill}")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetManifestHistoryForWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifestDTO = await _service.GetManifestHistoryForWaybill(waybill);

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = manifestDTO
                };
            });
        }
    }
}
