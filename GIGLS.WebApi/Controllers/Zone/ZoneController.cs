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
    [Authorize(Roles ="Admin")]
    [RoutePrefix("api/zone")]
    public class ZoneController : BaseWebApiController
    {
        private IZoneService _zoneService;
        public ZoneController(IZoneService zoneService) : base(nameof(ZoneController))
        {
            _zoneService = zoneService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ZoneDTO>>> GetZones()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zones = await _zoneService.GetZones();
                return new ServiceResponse<IEnumerable<ZoneDTO>>
                {
                    Object = zones
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("active")]
        public async Task<IServiceResponse<IEnumerable<ZoneDTO>>> GetActiveZones()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zones = await _zoneService.GetActiveZones();
                return new ServiceResponse<IEnumerable<ZoneDTO>>
                {
                    Object = zones
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<ZoneDTO>> GetZoneById(int zoneId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var station = await _zoneService.GetZoneById(zoneId);

                return new ServiceResponse<ZoneDTO>
                {
                    Object = station
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddZone(ZoneDTO newZone)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _zoneService.AddZone(newZone);
                return new ServiceResponse<object>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<object>> UpdateZone(int zoneId, ZoneDTO zoneDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _zoneService.UpdateZone(zoneId, zoneDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{zoneId:int}/status/{status}")]
        public async Task<IServiceResponse<object>> UpdateZone(int zoneId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _zoneService.UpdateZone(zoneId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<bool>> DeleteStation(int zoneId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _zoneService.DeleteZone(zoneId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
