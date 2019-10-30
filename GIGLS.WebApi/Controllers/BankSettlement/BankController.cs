using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.BankSettlement
{
    [AllowAnonymous]
    [RoutePrefix("api/bank")]
    public class BankController : BaseWebApiController
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService) : base(nameof(BankController))
        {
            _bankService = bankService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<BankDTO>>> GetBanks()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var banks = await _bankService.GetBanks();
                return new ServiceResponse<IEnumerable<BankDTO>>
                {
                    Object = banks
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddBank(BankDTO bankDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var bank = await _bankService.AddBank(bankDTO);

                return new ServiceResponse<object>
                {
                    Object = bank
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{bankId:int}")]
        public async Task<IServiceResponse<BankDTO>> GetBank(int bankId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var bank = await _bankService.GetBankById(bankId);

                return new ServiceResponse<BankDTO>
                {
                    Object = bank
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{bankId:int}")]
        public async Task<IServiceResponse<bool>> DeleteBank(int bankId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankService.DeleteBank(bankId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{bankId:int}")]
        public async Task<IServiceResponse<bool>> UpdateBank(int bankId, BankDTO bankDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankService.UpdateBank(bankId, bankDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}