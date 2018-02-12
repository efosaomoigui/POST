using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/country")]
    public class CountryController : BaseWebApiController
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService) : base(nameof(CountryController))
        {
            _countryService = countryService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CountryDTO>>> GetCountries()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var country = await _countryService.GetCountries();

                return new ServiceResponse<IEnumerable<CountryDTO>>
                {
                    Object = country
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCountry(CountryDTO countryDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var country = await _countryService.AddCountry(countryDTO);

                return new ServiceResponse<object>
                {
                    Object = country
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{countryId:int}")]
        public async Task<IServiceResponse<CountryDTO>> GetCountry(int countryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var country = await _countryService.GetCountryById(countryId);

                return new ServiceResponse<CountryDTO>
                {
                    Object = country
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{countryId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCountry(int countryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _countryService.DeleteCountry(countryId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{countryId:int}")]
        public async Task<IServiceResponse<bool>> UpdateCountry(int countryId, CountryDTO countryDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _countryService.UpdateCountry(countryId, countryDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
