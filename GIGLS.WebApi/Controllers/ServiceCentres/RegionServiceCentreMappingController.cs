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
    [RoutePrefix("api/regionServiceCentreMapping")]
    public class RegionServiceCentreMappingController : BaseWebApiController
    {
        private readonly IRegionServiceCentreMappingService _service;

        public RegionServiceCentreMappingController(IRegionServiceCentreMappingService service)
            : base(nameof(RegionServiceCentreMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<RegionServiceCentreMappingDTO>>> GetAllRegionServiceCentreMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var regionServiceCentreMappings = await _service.GetAllRegionServiceCentreMappings();

                return new ServiceResponse<List<RegionServiceCentreMappingDTO>>
                {
                    Object = regionServiceCentreMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("UnassignedServiceCentres")]
        public async Task<IServiceResponse<List<ServiceCentreDTO>>> GetUnassignedServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var unassignedServiceCentres = await _service.GetUnassignedServiceCentres();

                return new ServiceResponse<List<ServiceCentreDTO>>
                {
                    Object = unassignedServiceCentres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingServiceCentreToRegion(RegionServiceCentreMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingServiceCentreToRegion(data.RegionId, data.ServiceCentreIds);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ServiceCentreInRegion/{regionId}")]
        public async Task<IServiceResponse<List<RegionServiceCentreMappingDTO>>> GetServiceCentresInRegion(int regionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var serviceCentresInRegion = await _service.GetServiceCentresInRegion(regionId);

                return new ServiceResponse<List<RegionServiceCentreMappingDTO>>
                {
                    Object = serviceCentresInRegion
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RegionForServiceCentre/{serviceCentrId}")]
        public async Task<IServiceResponse<RegionServiceCentreMappingDTO>> GetRegionForServiceCentre(int serviceCentrId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var regionServiceCentreMappingDTO = await _service.GetRegionForServiceCentre(serviceCentrId);

                return new ServiceResponse<RegionServiceCentreMappingDTO>
                {
                    Object = regionServiceCentreMappingDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("RemoveServiceCentreFromRegion/{regionId}/{serviceCentrId}")]
        public async Task<IServiceResponse<bool>> RemoveServiceCentreFromRegion(int regionId, int serviceCentrId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.RemoveServiceCentreFromRegion(regionId, serviceCentrId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
