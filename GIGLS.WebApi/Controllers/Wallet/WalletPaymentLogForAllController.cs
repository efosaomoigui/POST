using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
{
    [Authorize]
    [RoutePrefix("api/walletpaymentlogforall")]
    public class WalletPaymentLogForAllController : BaseWebApiController
    {
        private readonly IWalletPaymentLogService _walletPaymentLogService;
        private readonly IWalletService _walletService;
        private readonly IWalletTransactionService _walletTransactionService;

        public WalletPaymentLogForAllController(IWalletPaymentLogService walletPaymentLogService, IWalletService walletService, IWalletTransactionService walletTransactionService) : base(nameof(WalletPaymentLogController))
        {
            _walletPaymentLogService = walletPaymentLogService;
            _walletService = walletService;
            _walletTransactionService = walletTransactionService;
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

        [HttpPost]
        [Route("getuserwallets")]
        public async Task<IServiceResponse<List<WalletDTO>>> GetUserWallets(WalletSearchOption searchDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletService.GetUserWallets(searchDTO);

                return new ServiceResponse<List<WalletDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("chargeuserwallet")]
        public async Task<IServiceResponse<bool>> ChargeUserWallets(WalletDTO walletDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletService.ChargeUserWallet(walletDTO);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("walletbydate")]
        public async Task<IServiceResponse<IEnumerable<WalletTransactionDTO>>> GetWalletTransactionsByDate(ShipmentCollectionFilterCriteria dateFilter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactions = await _walletTransactionService.GetWalletTransactionsByDate(dateFilter);
                return new ServiceResponse<IEnumerable<WalletTransactionDTO>>
                {
                    Object = walletTransactions
                };
            });
        }

    }
}