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
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipmentreroute")]
    public class ShipmentRerouteController : BaseWebApiController
    {
        private readonly IShipmentRerouteService _service;

        public ShipmentRerouteController(IShipmentRerouteService service) : base(nameof(ShipmentRerouteController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentRerouteDTO>>> GetRerouteShipments()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentReroutes = await _service.GetRerouteShipments();

                return new ServiceResponse<IEnumerable<ShipmentRerouteDTO>>
                {
                    Object = shipmentReroutes
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<ShipmentDTO>> AddShipmentReroute(ShipmentDTO shipmentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.AddRerouteShipment(shipmentDto);

                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<ShipmentRerouteDTO>> GetRerouteShipmentByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentReroute = await _service.GetRerouteShipment(waybill);

                return new ServiceResponse<ShipmentRerouteDTO>
                {
                    Object = shipmentReroute
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("")]
        public async Task<IServiceResponse<bool>> UpdateShipmentReroute(ShipmentRerouteDTO shipmentRerouteDto)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateRerouteShipment(shipmentRerouteDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeleteRerouteShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.DeleteRerouteShipment(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


    }
}
