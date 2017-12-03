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
    [RoutePrefix("api/haulagedistancemappingprice")]
    public class HaulageDistanceMappingPriceController : BaseWebApiController
    {
        private readonly IHaulageDistanceMappingPriceService _haulageDistanceMappingPriceService;

        public HaulageDistanceMappingPriceController(IHaulageDistanceMappingPriceService haulageDistanceMappingPriceService) : base(nameof(HaulageDistanceMappingPriceController))
        {
            _haulageDistanceMappingPriceService = haulageDistanceMappingPriceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<HaulageDistanceMappingPriceDTO>>> GetHaulageDistanceMappingPrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulageDistanceMappingPrice = await _haulageDistanceMappingPriceService.GetHaulageDistanceMappingPrices();

                return new ServiceResponse<IEnumerable<HaulageDistanceMappingPriceDTO>>
                {
                    Object = haulageDistanceMappingPrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddHaulageDistanceMappingPrice(HaulageDistanceMappingPriceDTO haulageDistanceMappingPriceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulageDistanceMappingPrice = await _haulageDistanceMappingPriceService.AddHaulageDistanceMappingPrice(haulageDistanceMappingPriceDto);

                return new ServiceResponse<object>
                {
                    Object = haulageDistanceMappingPrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{haulageDistanceMappingPriceId:int}")]
        public async Task<IServiceResponse<HaulageDistanceMappingPriceDTO>> GetHaulageDistanceMappingPriceById(int haulageDistanceMappingPriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulageDistanceMappingPrice = await _haulageDistanceMappingPriceService.GetHaulageDistanceMappingPriceById(haulageDistanceMappingPriceId);

                return new ServiceResponse<HaulageDistanceMappingPriceDTO>
                {
                    Object = haulageDistanceMappingPrice
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{haulageDistanceMappingPriceId:int}")]
        public async Task<IServiceResponse<bool>> RemoveHaulageDistanceMappingPrice(int haulageDistanceMappingPriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _haulageDistanceMappingPriceService.RemoveHaulageDistanceMappingPrice(haulageDistanceMappingPriceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{haulageDistanceMappingPriceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateHaulageDistanceMappingPrice(int zoneHaulagePriceId, HaulageDistanceMappingPriceDTO haulageDistanceMappingPriceDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _haulageDistanceMappingPriceService.UpdateHaulageDistanceMappingPrice(zoneHaulagePriceId, haulageDistanceMappingPriceDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
