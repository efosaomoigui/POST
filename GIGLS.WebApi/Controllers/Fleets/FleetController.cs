using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Vehicles
{
    [Authorize]
    [RoutePrefix("api/fleet")]
    public class FleetController : BaseWebApiController
    {
        private readonly IFleetService _service;

        public FleetController(IFleetService service) : base(nameof(FleetController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<FleetDTO>>> GetFleets()
        {
            return await HandleApiOperationAsync(async () => {
                var vehicles = await _service.GetFleets();
                return new ServiceResponse<IEnumerable<FleetDTO>>
                {
                    Object = vehicles
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddFleet(FleetDTO fleetDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vehicle = await _service.AddFleet(fleetDto);
                return new ServiceResponse<object>
                {
                    Object = vehicle
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{fleetId:int}")]
        public async Task<IServiceResponse<FleetDTO>> GetFleet(int fleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vehicle = await _service.GetFleetById(fleetId);
                return new ServiceResponse<FleetDTO>
                {
                    Object = vehicle
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{fleetId:int}")]
        public async Task<IServiceResponse<bool>> DeleteFleet(int fleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteFleet(fleetId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{fleetId:int}")]
        public async Task<IServiceResponse<bool>> UpdateFleet(int fleetId, FleetDTO fleetDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateFleet(fleetId, fleetDto);
                return new ServiceResponse<bool> { Object = true };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("capacity/{fleetId:int}")]
        public async Task<IServiceResponse<int>> GetFleetCapacity(int fleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                int capacity = await _service.GetFleetCapacity(fleetId);
                return new ServiceResponse<int>
                {
                    Object = capacity
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{fleetId:int}/capacity/{capacity:int}")]
        public async Task<IServiceResponse<bool>> SetFleetCapacity(int fleetId, int capacity)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.SetFleetCapacity(fleetId, capacity);
                return new ServiceResponse<bool> { Object = true };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{fleetId:int}/status/{status}")]
        public async Task<IServiceResponse<object>> UpdateFleetStatus(int fleetId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateFleetStatus(fleetId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

    }
}
