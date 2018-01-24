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
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/shipmentcollection")]
    public class ShipmentCollectionController : BaseWebApiController
    {
        private readonly IShipmentCollectionService _service;

        public ShipmentCollectionController(IShipmentCollectionService service) : base(nameof(ShipmentCollectionController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetAllShipmentCollections()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollections = await _service.GetShipmentCollections();

                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = shipmentCollections
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<ShipmentCollectionDTO>> GetShipmentCollectionByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollection = await _service.GetShipmentCollectionById(waybill);

                return new ServiceResponse<ShipmentCollectionDTO>
                {
                    Object = shipmentCollection
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeleteShipmentCollectionByCode(string waybill)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.RemoveShipmentCollection(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> AddShipmentCollection(ShipmentCollectionDTO shipmentCollection)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.AddShipmentCollection(shipmentCollection);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("")]
        public async Task<IServiceResponse<bool>> UpdateShipmentCollectionBywaybill(ShipmentCollectionDTO shipmentCollection)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateShipmentCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waitingforcollection")]
        public async Task<IServiceResponse<IEnumerable<ShipmentCollectionDTO>>> GetShipmentWaitingForCollection()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _service.GetShipmentWaitingForCollection();

                return new ServiceResponse<IEnumerable<ShipmentCollectionDTO>>
                {
                    Object = result
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("collected")]
        public async Task<IServiceResponse<bool>> UpdateShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKT;

            return await HandleApiOperationAsync(async () => {
                await _service.UpdateShipmentCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
