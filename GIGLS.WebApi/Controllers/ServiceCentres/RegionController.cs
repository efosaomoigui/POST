using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.WebApi.Controllers.ServiceCentres
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/region")]
    public class RegionController : BaseWebApiController
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService) : base(nameof(RegionController))
        {
            _regionService = regionService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<RegionDTO>>> GetRegions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var regions = await _regionService.GetRegions();
                return new ServiceResponse<IEnumerable<RegionDTO>>
                {
                    Object = regions
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddRegion(RegionDTO regionDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var region = await _regionService.AddRegion(regionDTO);

                return new ServiceResponse<object>
                {
                    Object = region
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{regionId:int}")]
        public async Task<IServiceResponse<RegionDTO>> GetRegion(int regionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var region = await _regionService.GetRegionById(regionId);

                return new ServiceResponse<RegionDTO>
                {
                    Object = region
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{regionId:int}")]
        public async Task<IServiceResponse<bool>> DeleteRegion(int regionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _regionService.DeleteRegion(regionId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{regionId:int}")]
        public async Task<IServiceResponse<bool>> UpdateRegion(int regionId, RegionDTO regionDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _regionService.UpdateRegion(regionId, regionDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
