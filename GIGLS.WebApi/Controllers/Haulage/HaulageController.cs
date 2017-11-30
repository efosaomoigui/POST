using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using GIGLS.Core.DTO.Haulage;

namespace GIGLS.WebApi.Controllers
{
    //[Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/haulage")]
    public class HaulageController : BaseWebApiController
    {
        private readonly IHaulageService _haulageService;

        public HaulageController(IHaulageService haulageService):base(nameof(HaulageController))
        {
            _haulageService = haulageService;
        }

       // [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<HaulageDTO>>> GetHaulages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulage = await _haulageService.GetHaulages();

                return new ServiceResponse<IEnumerable<HaulageDTO>>
                {
                    Object = haulage
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddHaulage(HaulageDTO haulageDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulage = await _haulageService.AddHaulage(haulageDto);

                return new ServiceResponse<object>
                {
                    Object = haulage
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{HaulageId:int}")]
        public async Task<IServiceResponse<HaulageDTO>> GetHaulage(int haulageId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulage = await _haulageService.GetHaulageById(haulageId);

                return new ServiceResponse<HaulageDTO>
                {
                    Object = haulage
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{HaulageId:int}")]
        public async Task<IServiceResponse<bool>> DeleteHaulage(int haulageId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _haulageService.RemoveHaulage(haulageId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{HaulageId:int}")]
        public async Task<IServiceResponse<bool>> UpdateHaulage(int haulageId, HaulageDTO haulageDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _haulageService.UpdateHaulage(haulageId, haulageDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
