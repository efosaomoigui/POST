using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/shipmentreturn")]
    public class ShipmentReturnController : BaseWebApiController
    {
        private readonly IShipmentReturnService _service;

        public ShipmentReturnController(IShipmentReturnService service) : base(nameof(ShipmentReturnController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentReturnDTO>>> GetAllShipmentReturns()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentReturns = await _service.GetShipmentReturns();

                return new ServiceResponse<IEnumerable<ShipmentReturnDTO>>
                {
                    Object = shipmentReturns
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<ShipmentReturnDTO>> GetShipmentReturnByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentReturn = await _service.GetShipmentReturnById(waybill);

                return new ServiceResponse<ShipmentReturnDTO>
                {
                    Object = shipmentReturn
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeleteShipmentReturnByCode(string waybill)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.RemoveShipmentReturn(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> AddShipmentReturn(ShipmentReturnDTO shipmentReturn)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.AddShipmentReturn(shipmentReturn);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> AddShipmentReturn(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.AddShipmentReturn(waybill);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("")]
        public async Task<IServiceResponse<bool>> UpdateShipmentReturnBywaybill(ShipmentReturnDTO shipmentReturn)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateShipmentReturn(shipmentReturn);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
