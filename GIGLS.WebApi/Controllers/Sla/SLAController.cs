using GIGLS.Core.DTO.SLA;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Sla;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Sla
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/sla")]
    public class SLAController : BaseWebApiController
    {
        private readonly ISLAService _service; 

        public SLAController(ISLAService service) : base(nameof(SLAController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<SLADTO>>> GetSLAs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SLA = await _service.GetSLAs();

                return new ServiceResponse<IEnumerable<SLADTO>>
                {
                    Object = SLA
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddSLA(SLADTO slaDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SLA = await _service.AddSLA(slaDto);

                return new ServiceResponse<object>
                {
                    Object = SLA
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{slaId:int}")]
        public async Task<IServiceResponse<SLADTO>> GetSLAById(int slaId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SLA = await _service.GetSLAById(slaId);

                return new ServiceResponse<SLADTO>
                {
                    Object = SLA
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{type}")]
        public async Task<IServiceResponse<SLADTO>> GetSLAByType(SLAType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SLA = await _service.GetSLAByType(type);

                return new ServiceResponse<SLADTO>
                {
                    Object = SLA
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{slaId:int}")]
        public async Task<IServiceResponse<bool>> DeleteSLA(int slaId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.RemoveSLA(slaId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{slaId:int}")]
        public async Task<IServiceResponse<bool>> UpdateSLA(int slaId, SLADTO slaDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateSLA(slaId, slaDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
