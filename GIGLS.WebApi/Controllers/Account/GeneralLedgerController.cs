using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Account
{
    [Authorize]
    [RoutePrefix("api/generalledger")]
    public class GeneralLedgerController : BaseWebApiController
    {
        private readonly IGeneralLedgerService _generalledgerService;
        public GeneralLedgerController(IGeneralLedgerService generalledgerService) : base(nameof(GeneralLedgerController))
        {
            _generalledgerService = generalledgerService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<GeneralLedgerDTO>>> GetGeneralLedgers()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var generalledger = await _generalledgerService.GetGeneralLedgers();

                return new ServiceResponse<IEnumerable<GeneralLedgerDTO>>
                {
                    Object = generalledger
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddGeneralLedger(GeneralLedgerDTO generalledgerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var generalledger = await _generalledgerService.AddGeneralLedger(generalledgerDto);

                return new ServiceResponse<object>
                {
                    Object = generalledger
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{generalledgerId:int}")]
        public async Task<IServiceResponse<GeneralLedgerDTO>> GetGeneralLedger(int generalledgerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var generalledger = await _generalledgerService.GetGeneralLedgerById(generalledgerId);

                return new ServiceResponse<GeneralLedgerDTO>
                {
                    Object = generalledger
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{generalledgerId:int}")]
        public async Task<IServiceResponse<bool>> DeleteGeneralLedger(int generalledgerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _generalledgerService.RemoveGeneralLedger(generalledgerId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{generalledgerId:int}")]
        public async Task<IServiceResponse<bool>> UpdateGeneralLedger(int generalledgerId, GeneralLedgerDTO generalledgerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _generalledgerService.UpdateGeneralLedger(generalledgerId, generalledgerDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
