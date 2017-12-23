using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using System.Collections.Generic;   
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using System.Linq;

namespace GIGLS.WebApi.Controllers.Account
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/vat")]
    public class VATController : BaseWebApiController
    {
        private readonly IVATService _vatService;
        public VATController(IVATService vatService) : base(nameof(VATController))
        {
            _vatService = vatService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<VATDTO>> GetVATs()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var vat = await _vatService.GetVATs();

                return new ServiceResponse<VATDTO>
                {
                    Object = vat.FirstOrDefault()
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddVAT(VATDTO vatDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vat = await _vatService.AddVAT(vatDto);

                return new ServiceResponse<object>
                {
                    Object = vat
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{vatId:int}")]
        public async Task<IServiceResponse<VATDTO>> GetVAT(int vatId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vat = await _vatService.GetVATById(vatId);

                return new ServiceResponse<VATDTO>
                {
                    Object = vat
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{vatId:int}")]
        public async Task<IServiceResponse<bool>> DeleteVAT(int vatId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _vatService.RemoveVAT(vatId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{vatId:int}")]
        public async Task<IServiceResponse<bool>> UpdateVAT(int vatId, VATDTO vatDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _vatService.UpdateVAT(vatId, vatDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
