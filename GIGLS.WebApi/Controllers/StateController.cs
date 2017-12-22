using GIGLS.Core.IServices;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "SuperAdmin,SubAdmin")]
    //[Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/state")]
    public class StateController : BaseWebApiController
    {
        private readonly IStateService _stateService;
        public StateController(IStateService stateService):base(nameof(StateController))
        {
            _stateService = stateService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<StateDTO>>> GetStates(int pageSize = 10, int page = 1)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _stateService.GetStates(pageSize, page);
                var total = _stateService.GetStatesTotal();

                return new ServiceResponse<IEnumerable<StateDTO>>
                {
                    Total = total,
                    Object = state
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddState(StateDTO stateDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _stateService.AddState(stateDto);

                return new ServiceResponse<object>
                {
                    Object = state
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{stateId:int}")]
        public async Task<IServiceResponse<StateDTO>> GetState(int stateId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _stateService.GetStateById(stateId);

                return new ServiceResponse<StateDTO>
                {
                    Object = state
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{stateId:int}")]
        public async Task<IServiceResponse<bool>> DeleteState(int stateId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _stateService.RemoveState(stateId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{stateId:int}")]
        public async Task<IServiceResponse<bool>> UpdateState(int stateId, StateDTO stateDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _stateService.UpdateState(stateId, stateDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
