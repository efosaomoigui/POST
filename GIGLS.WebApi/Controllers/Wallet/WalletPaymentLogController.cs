using POST.Core.DTO.Customers;
using POST.Core.DTO.OnlinePayment;
using POST.Core.DTO.Wallet;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Core.View;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/walletpaymentlog")]
    public class WalletPaymentLogController : BaseWebApiController
    {
        private readonly IWalletPaymentLogService _walletPaymentLogService;

        public WalletPaymentLogController(IWalletPaymentLogService walletPaymentLogService) : base(nameof(WalletPaymentLogController))
        {
            _walletPaymentLogService = walletPaymentLogService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<WalletPaymentLogView>>> GetWalletPaymentLogs([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTuple = _walletPaymentLogService.GetWalletPaymentLogs(filterOptionsDto);
                return new ServiceResponse<IEnumerable<WalletPaymentLogView>>
                {
                    Object = await walletTuple.Item1,
                    Total = walletTuple.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletPaymentLog = await _walletPaymentLogService.AddWalletPaymentLog(walletPaymentLogDTO);

                return new ServiceResponse<object>
                {
                    Object = walletPaymentLog
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{walletPaymentLogId:int}")]
        public async Task<IServiceResponse<WalletPaymentLogDTO>> GetWalletPaymentLog(int walletPaymentLogId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletPaymentLog = await _walletPaymentLogService.GetWalletPaymentLogById(walletPaymentLogId);

                return new ServiceResponse<WalletPaymentLogDTO>
                {
                    Object = walletPaymentLog
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{walletPaymentLogId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWalletPaymentLog(int walletPaymentLogId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletPaymentLogService.RemoveWalletPaymentLog(walletPaymentLogId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{reference}")]
        public async Task<IServiceResponse<bool>> UpdateWalletPaymentLog(string reference, WalletPaymentLogDTO walletPaymentLogDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletPaymentLogService.UpdateWalletPaymentLog(reference, walletPaymentLogDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpGet]
        [Route("verifypayment/{referenceCode}")]
        public async Task<IServiceResponse<PaymentResponse>> VerifyAndValidateWallet([FromUri]  string referenceCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletPaymentLogService.VerifyAndValidatePayment(referenceCode);

                return new ServiceResponse<PaymentResponse>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("walletlogs")]
        public async Task<IServiceResponse<List<WalletPaymentLogView>>> GetWalletPaymentLogFilter(DateFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletPaymentLogService.GetWalletPaymentLogs(filterCriteria);

                return new ServiceResponse<List<WalletPaymentLogView>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("searchwalletlogs")]
        public async Task<IServiceResponse<List<WalletPaymentLogView>>> GetWalletPaymentLogFilter(CompanySearchDTO searchDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletPaymentLogService.GetFromWalletPaymentLogViewBySearchParameter(searchDTO.searchItem);

                return new ServiceResponse<List<WalletPaymentLogView>>
                {
                    Object = result
                };
            });
        }

    }
}