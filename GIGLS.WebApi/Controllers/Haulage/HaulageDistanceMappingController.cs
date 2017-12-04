using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Haulage
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/haulagedistancemapping")]
    public class HaulageDistanceMappingController : BaseWebApiController
    {
        private readonly IHaulageDistanceMappingService _mapService;

        public HaulageDistanceMappingController(IHaulageDistanceMappingService mapService) : base(nameof(HaulageDistanceMappingController))
        {
            _mapService = mapService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<HaulageDistanceMappingDTO>>> GetHaulageDistanceMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulageMappings = await _mapService.GetHaulageDistanceMappings();
                return new ServiceResponse<IEnumerable<HaulageDistanceMappingDTO>>
                {
                    Object = haulageMappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddHaulageDistanceMapping(HaulageDistanceMappingDTO haulageDistanceMappingDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulageMapping = await _mapService.AddHaulageDistanceMapping(haulageDistanceMappingDTO);
                return new ServiceResponse<object>
                {
                    Object = haulageMapping
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{mappingId:int}")]
        public async Task<IServiceResponse<HaulageDistanceMappingDTO>> GetHaulageDistanceMappingById(int mappingId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _mapService.GetHaulageDistanceMappingById(mappingId);
                return new ServiceResponse<HaulageDistanceMappingDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{departure:int}/{destination:int}")]
        public async Task<IServiceResponse<HaulageDistanceMappingDTO>> GetHaulageDistanceMapping(int departure, int destination)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _mapService.GetHaulageDistanceMapping(departure, destination);

                return new ServiceResponse<HaulageDistanceMappingDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{mappingId:int}")]
        public async Task<IServiceResponse<bool>> DeleteHaulageDistanceMapping(int mappingId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapService.DeleteHaulageDistanceMapping(mappingId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{mappingId:int}")]
        public async Task<IServiceResponse<bool>> UpdateHaulageDistanceMapping(int mappingId, HaulageDistanceMappingDTO mappingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapService.UpdateHaulageDistanceMapping(mappingId, mappingDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{mappingId:int}/status/{status}")]
        public async Task<IServiceResponse<bool>> UpdateStatusHaulageDistanceMapping(int mappingId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapService.UpdateStatusHaulageDistanceMapping(mappingId, status);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }

}
