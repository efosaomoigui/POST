using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/captainbonusbyzonemapping")]
    public class CaptainBonusByZoneMapingController : BaseWebApiController
    {
        private readonly ICaptainBonusByZoneMapingService _mapingService;
        public CaptainBonusByZoneMapingController(ICaptainBonusByZoneMapingService mapingService) 
            : base(nameof(CaptainBonusByZoneMapingController))
        {
            _mapingService = mapingService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CaptainBonusByZoneMapingDTO>>> GetCaptainBonusByZoneMapping()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneMapping = await _mapingService.GetCaptainBonusByZoneMapping();
                return new ServiceResponse<IEnumerable<CaptainBonusByZoneMapingDTO>>
                {
                    Object = zoneMapping
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{zoneMappingId:int}")]
        public async Task<IServiceResponse<CaptainBonusByZoneMapingDTO>> GetCaptainBonusByZoneMappingId(int zoneMappingId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneMapping = await _mapingService.GetCaptainBonusByZoneMappingById(zoneMappingId);
                return new ServiceResponse<CaptainBonusByZoneMapingDTO>
                {
                    Object = zoneMapping
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{departureId:int}/{destinationId:int}")]
        public async Task<IServiceResponse<decimal>> GetCaptainBonusByZoneMappingzone(int departureId, int destinationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneMapping = await _mapingService.GetCaptainBonusByZoneMappingByZone(departureId, destinationId);
                return new ServiceResponse<decimal>
                {
                    Object = zoneMapping
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCaptainBonusByZoneMapping(CaptainBonusByZoneMapingDTO mappingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zoneMapping = await _mapingService.AddCaptainBonusByZoneMapping(mappingDto);
                return new ServiceResponse<object>
                {
                    Object = zoneMapping
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{zoneMappingId:int}")]
        public async Task<IServiceResponse<bool>> UpdateCaptainBonusByZoneMapping(int zoneMappingId, CaptainBonusByZoneMapingDTO mappingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapingService.UpdateCaptainBonusByZoneMapping(zoneMappingId, mappingDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{zoneMappingId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCaptainBonusByZoneMapping(int zoneMappingId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mapingService.DeleteCaptainBonusByZoneMapping(zoneMappingId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}