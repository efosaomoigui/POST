using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.DTO.Fleets;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Vehicles
{
    [Authorize]
    [RoutePrefix("api/fleettrip")]
    public class FleetTripController : BaseWebApiController
    {
        private readonly IFleetTripService _service;

        public FleetTripController(IFleetTripService service) : base(nameof(FleetTripController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<FleetTripDTO>>> GetFleetTrips()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vehicles = await _service.GetFleetTrips();
                return new ServiceResponse<IEnumerable<FleetTripDTO>>
                {
                    Object = vehicles
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddFleetTrip(FleetTripDTO fleettripDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vehicle = await _service.AddFleetTrip(fleettripDto);
                return new ServiceResponse<object>
                {
                    Object = vehicle
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{fleettripId:int}")]
        public async Task<IServiceResponse<FleetTripDTO>> GetFleetTrip(int fleettripId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vehicle = await _service.GetFleetTripById(fleettripId);
                return new ServiceResponse<FleetTripDTO>
                {
                    Object = vehicle
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{fleettripId:int}")]
        public async Task<IServiceResponse<bool>> DeleteFleetTrip(int fleettripId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteFleetTrip(fleettripId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{fleettripId:int}")]
        public async Task<IServiceResponse<bool>> UpdateFleetTrip(int fleettripId, FleetTripDTO fleettripDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateFleetTrip(fleettripId, fleettripDto);
                return new ServiceResponse<bool> { Object = true };
            });
        }

    }
}
