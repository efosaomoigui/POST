using GIGLS.Core.DTO.Utility;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Utility;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Utility
{

    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/globalproperty")]
    public class GlobalPropertyController : BaseWebApiController
    {

        private readonly IGlobalPropertyService _globalService;
        public GlobalPropertyController(IGlobalPropertyService globalService) :base(nameof(GlobalPropertyController))
        {
            _globalService = globalService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<GlobalPropertyDTO>>> GetGlobalProperties()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var globals = await _globalService.GetGlobalProperties();
                return new ServiceResponse<IEnumerable<GlobalPropertyDTO>>
                {
                    Object = globals
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddGlobalProperty(GlobalPropertyDTO globalPropertyDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var global = await _globalService.AddGlobalProperty(globalPropertyDto);

                return new ServiceResponse<object>
                {
                    Object = global
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{globalPropertyId:int}")]
        public async Task<IServiceResponse<GlobalPropertyDTO>> GetGlobalProperty(int globalPropertyId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var global = await _globalService.GetGlobalPropertyById(globalPropertyId);

                return new ServiceResponse<GlobalPropertyDTO>
                {
                    Object = global
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{globalPropertyId:int}")]
        public async Task<IServiceResponse<bool>> DeleteGlobalProperty(int globalPropertyId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _globalService.RemoveGlobalProperty(globalPropertyId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{globalPropertyId:int}")]
        public async Task<IServiceResponse<bool>> UpdateGlobalProperty(int globalPropertyId, GlobalPropertyDTO globalPropertyDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _globalService.UpdateGlobalProperty(globalPropertyId, globalPropertyDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
