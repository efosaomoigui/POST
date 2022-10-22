using POST.Core.DTO.Fleets;
using POST.Core.IServices;
using POST.Core.IServices.Fleets;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Fleets
{

    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/dispatchactivity")]
    public class DispatchActivityController : BaseWebApiController
    {
        private readonly IDispatchActivityService _dispatchActivityService;

        public DispatchActivityController(IDispatchActivityService service) : base(nameof(DispatchActivityController))
        {
            _dispatchActivityService = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DispatchActivityDTO>>> GetDispatchActivities()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatchActivities = await _dispatchActivityService.GetDispatchActivities();

                return new ServiceResponse<IEnumerable<DispatchActivityDTO>>
                {
                    Object = dispatchActivities
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDispatchActivity(DispatchActivityDTO dispatchActivityDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatchActivity = await _dispatchActivityService.AddDispatchActivity(dispatchActivityDTO);

                return new ServiceResponse<object>
                {
                    Object = dispatchActivity
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpGet]
        [Route("{dispatchActivityId:int}")]
        public async Task<IServiceResponse<DispatchActivityDTO>> GetDispatchActivity(int dispatchActivityId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatchActivity = await _dispatchActivityService.GetDispatchActivityById(dispatchActivityId);

                return new ServiceResponse<DispatchActivityDTO>
                {
                    Object = dispatchActivity
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{dispatchActivityId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDispatchActivity(int dispatchActivityId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _dispatchActivityService.DeleteDispatchActivity(dispatchActivityId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{dispatchActivityId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDispatchActivity(int dispatchActivityId, DispatchActivityDTO dispatchActivityDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _dispatchActivityService.UpdateDispatchActivity(dispatchActivityId, dispatchActivityDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
