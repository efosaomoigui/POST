using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Fleets
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/fleetmodel")]
    public class FleetModelController : BaseWebApiController
    {
        private readonly IFleetModelService _service;

        public FleetModelController(IFleetModelService service):base(nameof(FleetModelController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<FleetModelDTO>>> GetFleetModels()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Fleets = await _service.GetFleetModels();

                return new ServiceResponse<IEnumerable<FleetModelDTO>>
                {
                    Object = Fleets
                };
            });
           
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddFleetModel(FleetModelDTO FleetDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Fleet = await _service.AddFleetModel(FleetDto);
                return new ServiceResponse<object>
                {
                    Object = Fleet
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{FleetId:int}")]
        public async Task<IServiceResponse<FleetModelDTO>> GetFleetModel(int FleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Fleet = await _service.GetFleetModelById(FleetId);
                return new ServiceResponse<FleetModelDTO>
                {
                    Object = Fleet
                };
            });
            
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{FleetId:int}")]
        public async Task<IServiceResponse<bool>> DeleteFleetModel(int FleetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteFleetModel(FleetId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
               
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{FleetId:int}")]
        public async Task<IServiceResponse<bool>> UpdateFleetModel(int FleetId, FleetModelDTO FleetDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateFleetModel(FleetId, FleetDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
            
            
        }
    }
}
