using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.Haulage;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/zonehaulageprice")]
    public class ZoneHaulagePriceController : BaseWebApiController
    {
        private readonly IZoneHaulagePriceService _zoneHaulagePriceService;

        public ZoneHaulagePriceController(IZoneHaulagePriceService zoneHaulagePriceService):base(nameof(ZoneHaulagePriceController))
        {
            _zoneHaulagePriceService = zoneHaulagePriceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ZoneHaulagePriceDTO>>> GetZoneHaulagePrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneHaulagePrice = await _zoneHaulagePriceService.GetZoneHaulagePrices();

                return new ServiceResponse<IEnumerable<ZoneHaulagePriceDTO>>
                {
                    Object = zoneHaulagePrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddZoneHaulagePrice(ZoneHaulagePriceDTO zoneHaulagePriceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneHaulagePrice = await _zoneHaulagePriceService.AddZoneHaulagePrice(zoneHaulagePriceDto);

                return new ServiceResponse<object>
                {
                    Object = zoneHaulagePrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{zoneHaulagePriceId:int}")]
        public async Task<IServiceResponse<ZoneHaulagePriceDTO>> GetZoneHaulagePrice(int zoneHaulagePriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneHaulagePrice = await _zoneHaulagePriceService.GetZoneHaulagePriceById(zoneHaulagePriceId);

                return new ServiceResponse<ZoneHaulagePriceDTO>
                {
                    Object = zoneHaulagePrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{zoneHaulagePriceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteZoneHaulagePrice(int zoneHaulagePriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _zoneHaulagePriceService.RemoveZoneHaulagePrice(zoneHaulagePriceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{zoneHaulagePriceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateZoneHaulagePrice(int zoneHaulagePriceId, ZoneHaulagePriceDTO zoneHaulagePriceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _zoneHaulagePriceService.UpdateZoneHaulagePrice(zoneHaulagePriceId, zoneHaulagePriceDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
