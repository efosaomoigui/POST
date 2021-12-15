using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.ServiceCentres
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/placelocation")]
    public class PlaceLocationController : BaseWebApiController
    {
        private IPlaceLocationService _locationService;
        private IStationService _stationService;
        private IServiceCentreService _serviceCentreService;
        public PlaceLocationController(IPlaceLocationService locationService, IStationService stationService, IServiceCentreService serviceCentreService) : base(nameof(PlaceLocationController))
        {
            _locationService = locationService;
            _stationService = stationService;
            _serviceCentreService = serviceCentreService;
        }
        // GET: Location
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PlaceLocationDTO>>> GetLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var location = await _locationService.GetLocations();
                return new ServiceResponse<IEnumerable<PlaceLocationDTO>>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{stateId:int}/state")]
        public async Task<IServiceResponse<IEnumerable<PlaceLocationDTO>>> GetLocationsByState(int stateId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var location = await _locationService.GetLocationsByStateId(stateId);
                return new ServiceResponse<IEnumerable<PlaceLocationDTO>>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{serviceCentreId:int}/servicecentre")]
        public async Task<IServiceResponse<IEnumerable<PlaceLocationDTO>>> GetLocationsByServiceCentre(int serviceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var serviceCenter = await _serviceCentreService.GetServiceCentreById(serviceCentreId);
                var station = await _stationService.GetStationById(serviceCenter.StationId);
                var location = await _locationService.GetLocationsByStateId(station.StateId);
                return new ServiceResponse<IEnumerable<PlaceLocationDTO>>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{locationId:int}")]
        public async Task<IServiceResponse<PlaceLocationDTO>> GetLocationById(int locationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var location = await _locationService.GetLocationById(locationId);
                return new ServiceResponse<PlaceLocationDTO>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddLocation(PlaceLocationDTO locationDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var location = await _locationService.AddLocation(locationDto);
                return new ServiceResponse<object>
                {
                    Object = location
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}")]
        public async Task<IServiceResponse<object>> UpdateLocation(int locationId, PlaceLocationDTO locationDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.UpdateLocation(locationId, locationDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{locationId:int}")]
        public async Task<IServiceResponse<bool>> DeleteLGA(int locationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.DeleteLocation(locationId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}/homedeliverystatus/{status}")]
        public async Task<IServiceResponse<object>> UpdateHomeDeliveryStatus(int locationId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.UpdateHomeDeliveryLocation(locationId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}/normalhomedeliverystatus/{status}")]
        public async Task<IServiceResponse<object>> UpdateNormalHomeDeliveryStatus(int locationId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.UpdateNormalHomeDeliveryLocation(locationId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}/giggostatus/{status}")]
        public async Task<IServiceResponse<object>> UpdateGiggoStatus(int locationId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.UpdateGIGGOLocation(locationId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}/extramiledeliverystatus/{status}")]
        public async Task<IServiceResponse<object>> UpdateExtraMileDeliveryStatus(int locationId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.UpdateExtraMileDeliveryLocation(locationId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{locationId:int}/expresshomedeliverystatus/{status}")]
        public async Task<IServiceResponse<object>> UpdateExpressHomeDeliveryStatus(int locationId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _locationService.UpdateExpressHomeDeliveryLocation(locationId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{stationId:int}/station")]
        public async Task<IServiceResponse<IEnumerable<PlaceLocationDTO>>> GetLocationsByStation(int stationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var station = await _stationService.GetStationById(stationId);
                var location = await _locationService.GetLocationsByStateId(station.StateId);
                return new ServiceResponse<IEnumerable<PlaceLocationDTO>>
                {
                    Object = location
                };
            });
        }
    }
}
