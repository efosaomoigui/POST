using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/shipment")]
    public class ShipmentController : BaseWebApiController
    {
        private readonly IShipmentService _service;

        public ShipmentController(IShipmentService service) : base(nameof(ShipmentController))
        {
            _service = service;
        }


        //public Task<IEnumerable<StateDTO>> GetStatesAsync(int pageSize = 10, int page = 1)
        //{
        //    var states = Context.State.ToList();
        //    var subresult = states.Skip(pageSize * (page - 1)).Take(pageSize);
        //    var stateDto = Mapper.Map<IEnumerable<StateDTO>>(subresult);
        //    return Task.FromResult(stateDto);
        //}

        //[HttpGet]
        //[Route("all")]
        //public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetShipments()
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var shipments = await _service.GetShipments();
        //        return new ServiceResponse<IEnumerable<ShipmentDTO>>
        //        {
        //            Object = shipments
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetShipments([FromUri]FilterOptionsDto filterOptionsDto) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments =  _service.GetShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentDTO>>
                {
                    Object = await shipments.Item1,
                    Total =  shipments.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("incomingshipments")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetIncomingShipments([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _service.GetIncomingShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentDTO>>
                {
                    Object = shipments,
                    Total = shipments.Count
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<ShipmentDTO>> AddShipment(ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.AddShipment(ShipmentDTO);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{ShipmentId:int}")]
        public async Task<IServiceResponse<ShipmentDTO>> GetShipment(int ShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.GetShipment(ShipmentId);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<ShipmentDTO>> GetShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.GetShipment(waybill);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{ShipmentId:int}")]
        public async Task<IServiceResponse<bool>> DeleteShipment(int ShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteShipment(ShipmentId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<bool>> DeleteShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteShipment(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{shipmentId:int}")]
        public async Task<IServiceResponse<bool>> UpdateShipment(int shipmentId, ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateShipment(shipmentId, ShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> UpdateShipment(string waybill, ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateShipment(waybill, ShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ungroupedwaybillsforservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetUnGroupedWaybillsForServiceCentre([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _service.GetUnGroupedWaybillsForServiceCentre(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentDTO>>
                {
                    Object = shipments,
                    Total = shipments.Count
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ungroupmappingservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUnGroupMappingServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetUnGroupMappingServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedgroupedwaybillsforservicecentre")]
        public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>> GetUnmappedGroupedWaybillsForServiceCentre([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var unmappedGroupWaybills = await _service.GetUnmappedGroupedWaybillsForServiceCentre(filterOptionsDto);
                return new ServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>
                {
                    Object = unmappedGroupWaybills,
                    Total = unmappedGroupWaybills.Count
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedmanifestservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUnmappedManifestServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetUnmappedManifestServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("zone/{destinationServiceCentreId:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int destinationServiceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _service.GetZone(destinationServiceCentreId);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("dailysales")]
        public async Task<IServiceResponse<DailySalesDTO>> GetDailySales(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var dailySales = await _service.GetDailySales(accountFilterCriteria);

                return new ServiceResponse<DailySalesDTO>
                {
                    Object = dailySales
                };
            });
        }

    }
}
