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
    [RoutePrefix("api/shipmentcancel")]
    public class ShipmentCancelController : BaseWebApiController
    {
        private readonly IShipmentCancelService _service;

        public ShipmentCancelController(IShipmentCancelService service) : base(nameof(ShipmentCancelController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<ShipmentCancelDTO>>> GetAllShipmentCancel()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentcancelled = await _service.GetShipmentCancels();

                return new ServiceResponse<List<ShipmentCancelDTO>>
                {
                    Object = shipmentcancelled
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<ShipmentCancelDTO>> GetShipmentCancelByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentcancelled = await _service.GetShipmentCancelById(waybill);

                return new ServiceResponse<ShipmentCancelDTO>
                {
                    Object = shipmentcancelled
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> AddShipmentCancel(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.AddShipmentCancel(waybill);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


    }
}
