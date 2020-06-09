using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/walletTransaction")]
    public class WalletTransactionController : BaseWebApiController
    {
        private readonly IWalletTransactionService _walletTransactionService;
        public WalletTransactionController(IWalletTransactionService walletTransactionService) : base(nameof(WalletTransactionController))
        {
            _walletTransactionService = walletTransactionService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<WalletTransactionDTO>>> GetWalletTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactions = await _walletTransactionService.GetWalletTransactions();
                return new ServiceResponse<IEnumerable<WalletTransactionDTO>>
                {
                    Object = walletTransactions
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("credit")]
        public async Task<IServiceResponse<List<WalletTransactionDTO>>> GetWalletTransactionsCredit(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactions = await _walletTransactionService.GetWalletTransactionsCredit(accountFilterCriteria);
                return new ServiceResponse<List<WalletTransactionDTO>>
                {
                    Object = walletTransactions
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{walletTransactionId:int}")]
        public async Task<IServiceResponse<WalletTransactionDTO>> GetWalletTransactionById(int walletTransactionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransaction = await _walletTransactionService.GetWalletTransactionById(walletTransactionId);

                return new ServiceResponse<WalletTransactionDTO>
                {
                    Object = walletTransaction
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{walletId:int}/summary")]
        public async Task<IServiceResponse<WalletTransactionSummaryDTO>> GetWalletTransactionByWalletId(int walletId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactionSummary = await _walletTransactionService.GetWalletTransactionByWalletId(walletId);

                return new ServiceResponse<WalletTransactionSummaryDTO>
                {
                    Object = walletTransactionSummary
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddWalletTransaction(WalletTransactionDTO newWalletTransaction)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletTransactionService.AddWalletTransaction(newWalletTransaction);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{walletTransactionId:int}")]
        public async Task<IServiceResponse<object>> UpdateWalletTransaction(int walletTransactionId, WalletTransactionDTO walletTransactionDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletTransactionService.UpdateWalletTransaction(walletTransactionId, walletTransactionDTO);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{walletTransactionId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWalletTransaction(int walletTransactionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletTransactionService.RemoveWalletTransaction(walletTransactionId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
