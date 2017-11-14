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
    [Authorize]
    [RoutePrefix("api/domesticzoneprice")]
    public class DomesticZonePriceController : BaseWebApiController 
    {

        private readonly IDomesticZonePriceService _service;

        public DomesticZonePriceController(IDomesticZonePriceService service):base(nameof(DomesticZonePriceController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DomesticZonePriceDTO>>> GetDomesticZones()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zones = await _service.GetDomesticZonePrices();
                return new ServiceResponse<IEnumerable<DomesticZonePriceDTO>>
                {
                    Object = zones
                };
            });
            
            
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDomesticZone(DomesticZonePriceDTO zoneDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _service.AddDomesticZonePrice(zoneDto);
                return new ServiceResponse<object>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<DomesticZonePriceDTO>> GetDomesticZone(int zoneId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               var zone = await _service.GetDomesticZonePriceById(zoneId);
               return new ServiceResponse<DomesticZonePriceDTO>
               {
                   Object = zone
               };
           });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDomesticZone(int zoneId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               await _service.DeleteDomesticZonePrice(zoneId);

               return new ServiceResponse<bool>
               {
                   Object = true
               };
               
           });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{zoneId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDomesticZone(int zoneId, DomesticZonePriceDTO zoneDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateDomesticZonePrice(zoneId, zoneDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
