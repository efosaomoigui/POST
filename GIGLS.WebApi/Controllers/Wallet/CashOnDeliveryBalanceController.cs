using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CashOnDeliveryBalance;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CashOnDeliveryBalance
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/cashondeliverybalance")]
    public class CashOnDeliveryBalanceController : BaseWebApiController
    {
        private readonly ICashOnDeliveryBalanceService _cashOnDeliveryBalanceService;

        public CashOnDeliveryBalanceController(ICashOnDeliveryBalanceService cashOnDeliveryBalanceService) :base(nameof(CashOnDeliveryBalanceController))
        {
            _cashOnDeliveryBalanceService = cashOnDeliveryBalanceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CashOnDeliveryBalanceDTO>>> GetCashOnDeliveryBalances()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var cashOnDeliveryBalances = await _cashOnDeliveryBalanceService.GetCashOnDeliveryBalances();
                return new ServiceResponse<IEnumerable<CashOnDeliveryBalanceDTO>>
                {
                    Object = cashOnDeliveryBalances
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{cashOnDeliveryBalanceId:int}")]
        public async Task<IServiceResponse<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalanceById(int cashOnDeliveryBalanceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cashOnDeliveryBalanceService.GetCashOnDeliveryBalanceById(cashOnDeliveryBalanceId);

                return new ServiceResponse<CashOnDeliveryBalanceDTO>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{walletNumber}/wallet")]
        public async Task<IServiceResponse<List<CashOnDeliveryBalanceDTO>>> GetCashOnDeliveryBalanceByWallet(string walletNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cashOnDeliveryBalanceService.GetCashOnDeliveryBalanceByWallet(walletNumber);

                return new ServiceResponse<List<CashOnDeliveryBalanceDTO>>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCashOnDeliveryBalance(CashOnDeliveryBalanceDTO newCashOnDeliveryBalance)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _cashOnDeliveryBalanceService.AddCashOnDeliveryBalance(newCashOnDeliveryBalance);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{cashOnDeliveryBalanceId:int}")]
        public async Task<IServiceResponse<object>> UpdateCashOnDeliveryBalance(int cashOnDeliveryBalanceId, CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _cashOnDeliveryBalanceService.UpdateCashOnDeliveryBalance(cashOnDeliveryBalanceId, cashOnDeliveryBalanceDTO);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{cashOnDeliveryBalanceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCashOnDeliveryBalance(int cashOnDeliveryBalanceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _cashOnDeliveryBalanceService.RemoveCashOnDeliveryBalance(cashOnDeliveryBalanceId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
