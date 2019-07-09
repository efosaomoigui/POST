using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.dispatchs
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/dispatch")]
    public class DispatchController : BaseWebApiController
    {
        private readonly IDispatchService _dispatchService;

        public DispatchController(IDispatchService dispatchService) : base(nameof(DispatchController))
        {
            _dispatchService = dispatchService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DispatchDTO>>> GetDispatchs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatchs = await _dispatchService.GetDispatchs();

                return new ServiceResponse<IEnumerable<DispatchDTO>>
                {
                    Object = dispatchs
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDispatch(DispatchDTO dispatchDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatch = await _dispatchService.AddDispatch(dispatchDTO);

                return new ServiceResponse<object>
                {
                    Object = dispatch
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{dispatchId:int}")]
        public async Task<IServiceResponse<DispatchDTO>> GetDispatch(int dispatchId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatch = await _dispatchService.GetDispatchById(dispatchId);

                return new ServiceResponse<DispatchDTO>
                {
                    Object = dispatch
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{manifest}/manifest")]
        public async Task<IServiceResponse<DispatchDTO>> GetDispatchManifestCode(string manifest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatch = await _dispatchService.GetDispatchManifestCode(manifest);

                return new ServiceResponse<DispatchDTO>
                {
                    Object = dispatch
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("captain")]
        public async Task<IServiceResponse<List<DispatchDTO>>> GetDispatchCaptainByName(string captain)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dispatch = await _dispatchService.GetDispatchCaptainByName(captain);

                return new ServiceResponse<List<DispatchDTO>>
                {
                    Object = dispatch
            };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{dispatchId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDispatch(int dispatchId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _dispatchService.DeleteDispatch(dispatchId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{dispatchId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDispatch(int dispatchId, DispatchDTO dispatchDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _dispatchService.UpdateDispatch(dispatchId, dispatchDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
