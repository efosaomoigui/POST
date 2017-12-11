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
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/specialdomesticzoneprice")]
    public class SpecialDomesticZonePriceController : BaseWebApiController
    {
        private readonly ISpecialDomesticZonePriceService _specialZonePrice;
        public SpecialDomesticZonePriceController(ISpecialDomesticZonePriceService specialZoneService):base(nameof(SpecialDomesticZonePriceController))
        {
            _specialZonePrice = specialZoneService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<SpecialDomesticZonePriceDTO>>> GetSpecialDomesticZonePrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var prices = await  _specialZonePrice.GetSpecialDomesticZonePrices();

                return new ServiceResponse<IEnumerable<SpecialDomesticZonePriceDTO>>
                {
                    Object = prices
                };

            });

        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{specialDomesticZonePriceId:int}")]
        public async Task<IServiceResponse<SpecialDomesticZonePriceDTO>> GetSpecialDomesticZonePrice(int specialDomesticZonePriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _specialZonePrice.GetSpecialDomesticZonePriceById(specialDomesticZonePriceId);
                return new ServiceResponse<SpecialDomesticZonePriceDTO>
                {
                    Object = price
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddSpecialDomesticZonePrice(SpecialDomesticZonePriceDTO newSpecialDomesticZonePrice)
        {

            return await HandleApiOperationAsync(async () => {
                var price = await _specialZonePrice.AddSpecialDomesticZonePrice(newSpecialDomesticZonePrice);
                return new ServiceResponse<object>
                {
                    Object = price
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{specialDomesticZonePriceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateSpecialDomesticZonePrice(int specialDomesticZonePriceId, SpecialDomesticZonePriceDTO specialZonePriceDTO)
        {

            return await HandleApiOperationAsync(async () =>
            {
                await _specialZonePrice.UpdateSpecialDomesticZonePrice(specialDomesticZonePriceId, specialZonePriceDTO);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{specialDomesticZonePriceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteSpecialDomesticZonePrice(int specialDomesticZonePriceId) {

            return await HandleApiOperationAsync(async () =>
            {
                await _specialZonePrice.DeleteSpecialDomesticZonePrice(specialDomesticZonePriceId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });

        }
            
    }
}
