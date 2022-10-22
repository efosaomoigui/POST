using POST.Core.DTO.Shipments;
using POST.Core.IServices;
using POST.Core.IServices.Shipments;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Shipments
{
    [Authorize(Roles = ("Admin, ViewAdmin"))]
    [RoutePrefix("api/logvisitreason")]
    public class LogVisitReasonController : BaseWebApiController
    {
        private readonly ILogVisitReasonService _service;

        public LogVisitReasonController(ILogVisitReasonService service) : base(nameof(LogVisitReasonController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<LogVisitReasonDTO>>> GetLogVisitReasons()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var logVisitReasons = await _service.GetLogVisitReasons();

                return new ServiceResponse<List<LogVisitReasonDTO>>
                {
                    Object = logVisitReasons
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddLogVisitReason(LogVisitReasonDTO logVisitReasonDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var logVisitReason = await _service.AddLogVisitReason(logVisitReasonDto);

                return new ServiceResponse<object>
                {
                    Object = logVisitReason
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{logVisitReasonId:int}")]
        public async Task<IServiceResponse<LogVisitReasonDTO>> GetLogVisitReason(int logVisitReasonId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var logVisitReason = await _service.GetLogVisitReasonById(logVisitReasonId);

                return new ServiceResponse<LogVisitReasonDTO>
                {
                    Object = logVisitReason
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{logVisitReasonId:int}")]
        public async Task<IServiceResponse<bool>> DeleteLogVisitReason(int logVisitReasonId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteLogVisitReason(logVisitReasonId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{logVisitReasonId:int}")]
        public async Task<IServiceResponse<bool>> UpdateLogVisitReason(int logVisitReasonId, LogVisitReasonDTO logVisitReasonDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateLogVisitReason(logVisitReasonId, logVisitReasonDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
