using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.ServiceCentres
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/homedeliverylocations")]
    public class HomeDeliveryLocationController: BaseWebApiController
    {
        private IHomeDeliveryLocationService _homeDeliveryLocationService;
        public HomeDeliveryLocationController(IHomeDeliveryLocationService homeDeliveryLocationService) : base(nameof(HomeDeliveryLocationController))
        {
            _homeDeliveryLocationService = homeDeliveryLocationService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<HomeDeliveryLocationDTO>>> GetHomeDeliveryLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _homeDeliveryLocationService.GetHomeDeliveryLocations();
                return new ServiceResponse<IEnumerable<HomeDeliveryLocationDTO>>
                {
                    Object = locations
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("active")]
        public async Task<IServiceResponse<IEnumerable<HomeDeliveryLocationDTO>>> GetActiveLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _homeDeliveryLocationService.GetActiveLocations();
                return new ServiceResponse<IEnumerable<HomeDeliveryLocationDTO>>
                {
                    Object = locations

                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{locationId:int}")]
        public async Task<IServiceResponse<HomeDeliveryLocationDTO>> GetHomeDeliveryLocationById(int locationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var location = await _homeDeliveryLocationService.GetHomeDeliveryLocationById(locationId);
                return new ServiceResponse<HomeDeliveryLocationDTO>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddHomeDeliveryLocation(HomeDeliveryLocationDTO locationDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var location = await _homeDeliveryLocationService.AddHomeDeliveryLocation(locationDTO);
                return new ServiceResponse<object>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}")]
        public async Task<IServiceResponse<object>> UpdateHomeDeliveryLocation(int locationId, HomeDeliveryLocationDTO locationDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _homeDeliveryLocationService.UpdateHomeDeliveryLocation(locationId, locationDTO);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}/status/{status}")]
        public async Task<IServiceResponse<object>> UpdateHomeDeliveryLocation(int locationId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _homeDeliveryLocationService.UpdateHomeDeliveryLocation(locationId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{locationId:int}")]
        public async Task<IServiceResponse<bool>> DeleteHomeDeliveryLocation(int locationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _homeDeliveryLocationService.DeleteHomeDeliveryLocation(locationId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}