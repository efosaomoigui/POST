using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Zone
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/weightlimitprice")]
    public class WeightLimitPriceController : BaseWebApiController
    {
        private readonly IWeightLimitPriceService _weightService;

        public WeightLimitPriceController(IWeightLimitPriceService weightService) : base(nameof(WeightLimitPriceController))
        {
            _weightService = weightService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<WeightLimitPriceDTO>>> GetWeightLimitPrice()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var weightLimit = await _weightService.GetWeightLimitPrices();
                return new ServiceResponse<IEnumerable<WeightLimitPriceDTO>>
                {
                    Object = weightLimit
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{weightLimitPriceId:int}")]
        public async Task<IServiceResponse<WeightLimitPriceDTO>> GetWeightLimitPriceId(int weightLimitId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var weightLimit = await _weightService.GetWeightLimitPriceById(weightLimitId);

                return new ServiceResponse<WeightLimitPriceDTO>
                {
                    Object = weightLimit
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<WeightLimitPriceDTO>> GetWeightLimitPriceByZoneId(int zoneId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var weightLimit = await _weightService.GetWeightLimitPriceByZoneId(zoneId);

                return new ServiceResponse<WeightLimitPriceDTO>
                {
                    Object = weightLimit
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddWeightLimitPrice(WeightLimitPriceDTO weightLimitDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var weightLimit = await _weightService.AddWeightLimitPrice(weightLimitDto);
                return new ServiceResponse<object>
                {
                    Object = weightLimit
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{weightLimitPriceId:int}")]
        public async Task<IServiceResponse<object>> UpdateWeightLimitPrice(int weightLimitPriceId, WeightLimitPriceDTO weightLimitDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _weightService.UpdateWeightLimitPrice(weightLimitPriceId, weightLimitDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{weightLimitPriceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWeightLimitPrice(int weightLimitPriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _weightService.RemoveWeightLimitPrice(weightLimitPriceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
