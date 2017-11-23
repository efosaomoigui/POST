using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Fleets
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/fleetmanagement")]
    public class FleetManagementController : BaseWebApiController
    {
        private readonly IFleetManagementService _fleetService;

        public FleetManagementController(IFleetManagementService fleetService) : base(nameof(FleetManagementController))
        {
            _fleetService = fleetService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<FleetLocationDTO>>> GetFleetLocations()
        {
            return await HandleApiOperationAsync(async () => {
                var fleets = await _fleetService.GetFleetLocations();
                return new ServiceResponse<IEnumerable<FleetLocationDTO>>
                {
                    Object = fleets
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{fleetId:int}")]
        public async Task<IServiceResponse<FleetLocationDTO>> GetFleetLocationById(int fleetId)
        {
            return await HandleApiOperationAsync(async () => {
                var fleet = await _fleetService.GetFleetLocationById(fleetId);
                return new ServiceResponse<FleetLocationDTO>
                {
                    Object = fleet
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("{fleetId:int}")]
        public async Task<IServiceResponse<IEnumerable<FleetLocationDTO>>> GetFleetLocationInformation(int fleetId, DateTime date)
        {
            return await HandleApiOperationAsync(async () => {
                var fleets = await _fleetService.GetFleetLocationInformation(fleetId, date);
                return new ServiceResponse<IEnumerable<FleetLocationDTO>>
                {
                    Object = fleets
                };
            });
        }

    }
}
