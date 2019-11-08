using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/countryrateconversion")]
    public class CountryRateConversionController : BaseWebApiController
    {
        private readonly ICountryRateConversionService _countryConversionService;

        public CountryRateConversionController(ICountryRateConversionService countryRateConversionService) : base(nameof(CountryRateConversionController))
        {
            _countryConversionService = countryRateConversionService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CountryRateConversionDTO>>> GetCountryRateConversion()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var countryRates = await _countryConversionService.GetCountryRateConversion();
                return new ServiceResponse<IEnumerable<CountryRateConversionDTO>>
                {
                    Object = countryRates
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{CountryRateConversionId:int}")]
        public async Task<IServiceResponse<CountryRateConversionDTO>> GetCountryRateConversionById(int CountryRateConversionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _countryConversionService.GetCountryRateConversionById(CountryRateConversionId);

                return new ServiceResponse<CountryRateConversionDTO>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{departureCountryId}/{destinationCountryId}")]
        public async Task<IServiceResponse<decimal>> GetCountryRateConversionRate(int departureCountryId, int destinationCountryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _countryConversionService.GetCountryRateConversionRate(departureCountryId, destinationCountryId);

                return new ServiceResponse<decimal>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCountryRateConversion(CountryRateConversionDTO countryRateConversion)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _countryConversionService.AddCountryRateConversion(countryRateConversion);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{CountryRateConversionId:int}")]
        public async Task<IServiceResponse<object>> UpdateWallet(int CountryRateConversionId, CountryRateConversionDTO countryRateConversion)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _countryConversionService.UpdateCountryRateConversion(CountryRateConversionId, countryRateConversion);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{CountryRateConversionId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWallet(int CountryRateConversionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _countryConversionService.DeleteCountryRateConversion(CountryRateConversionId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
