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
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/shipmentpackageprice")]
    public class ShipmentPackagePriceController : BaseWebApiController
    {

        private readonly IShipmentPackagePriceService _packagePriceService;
        public ShipmentPackagePriceController(IShipmentPackagePriceService packagePriceService) : base(nameof(ShipmentPackagePriceController))
        {
            _packagePriceService = packagePriceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentPackagePriceDTO>>> GetShipmentPackagePrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentPackagePrices = await _packagePriceService.GetShipmentPackagePrices();

                return new ServiceResponse<IEnumerable<ShipmentPackagePriceDTO>>
                {
                    Object = shipmentPackagePrices
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddShipmentPackagePrice(ShipmentPackagePriceDTO shipmentPackagePriceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentPackagePrice = await _packagePriceService.AddShipmentPackagePrice(shipmentPackagePriceDTO);

                return new ServiceResponse<object>
                {
                    Object = shipmentPackagePrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{shipmentPackagePriceId:int}")]
        public async Task<IServiceResponse<ShipmentPackagePriceDTO>> GetShipmentPackagePrice(int shipmentPackagePriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentPackagePrice = await _packagePriceService.GetShipmentPackagePriceById(shipmentPackagePriceId);

                return new ServiceResponse<ShipmentPackagePriceDTO>
                {
                    Object = shipmentPackagePrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{shipmentPackagePriceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteShipmentPackagePrice(int shipmentPackagePriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _packagePriceService.DeleteShipmentPackagePrice(shipmentPackagePriceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{shipmentPackagePriceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateShipmentPackagePrice(int shipmentPackagePriceId, ShipmentPackagePriceDTO shipmentPackagePriceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _packagePriceService.UpdateShipmentPackagePrice(shipmentPackagePriceId, shipmentPackagePriceDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("country")]
        public async Task<IServiceResponse<IEnumerable<ShipmentPackagePriceDTO>>> GetShipmentPackagePriceByCountry()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentPackagePrices = await _packagePriceService.GetShipmentPackagePriceByCountry();

                return new ServiceResponse<IEnumerable<ShipmentPackagePriceDTO>>
                {
                    Object = shipmentPackagePrices
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPut]
        [Route("packagetopup/{shipmentPackagePriceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateShipmentPackageQuantity(int shipmentPackagePriceId, ShipmentPackagePriceDTO shipmentPackagePriceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _packagePriceService.UpdateShipmentPackageQuantity(shipmentPackagePriceId, shipmentPackagePriceDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("addpackage")]
        public async Task<IServiceResponse<object>> AddShipmentPackage(ShipmentPackagePriceDTO shipmentPackagePriceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentPackagePrice = await _packagePriceService.AddShipmentPackage(shipmentPackagePriceDTO);

                return new ServiceResponse<object>
                {
                    Object = shipmentPackagePrice
                };
            });
        }

    }
}
