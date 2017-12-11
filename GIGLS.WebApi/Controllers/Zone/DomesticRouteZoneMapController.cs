using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Zone
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/domesticroutezonemap")]
    public class DomesticRouteZoneMapController : BaseWebApiController
    {
        private readonly IDomesticRouteZoneMapService _mapService;
        public DomesticRouteZoneMapController(IDomesticRouteZoneMapService mapService):base(nameof(DomesticRouteZoneMapController))
        {
            _mapService = mapService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DomesticRouteZoneMapDTO>>> GetDomesticRouteZoneMaps()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zones = await _mapService.GetRouteZoneMaps();
                return new ServiceResponse<IEnumerable<DomesticRouteZoneMapDTO>>
                {
                    Object = zones
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDomesticZone(DomesticRouteZoneMapDTO zoneDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _mapService.AddRouteZoneMap(zoneDto);
                return new ServiceResponse<object>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{mappingId:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetDomesticZone(int mappingId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _mapService.GetRouteZoneMapById(mappingId);
                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{departure:int}/{destination:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int departure, int destination)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _mapService.GetZone(departure, destination);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{mappingId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDomesticZone(int mappingId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapService.DeleteRouteZoneMap(mappingId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{mappingId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDomesticZone(int mappingId, DomesticRouteZoneMapDTO mappingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapService.UpdateRouteZoneMap(mappingId, mappingDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{mappingId:int}/status/{status}")]
        public async Task<IServiceResponse<bool>> UpdateStatus(int mappingId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapService.UpdateStatusRouteZoneMap(mappingId, status);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
