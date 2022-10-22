using POST.Core.DTO.Wallet;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POST.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/wallet")]
    public class WalletController : BaseWebApiController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService) : base(nameof(WalletController))
        {
            _walletService = walletService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<WalletDTO>>> GetWallets()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var wallets = await _walletService.GetWallets();
                return new ServiceResponse<IEnumerable<WalletDTO>>
                {
                    Object = wallets
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{walletId:int}")]
        public async Task<IServiceResponse<WalletDTO>> GetWalletById(int walletId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletService.GetWalletById(walletId);

                return new ServiceResponse<WalletDTO>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getsystemwallet")]
        public async Task<IServiceResponse<WalletDTO>> GetSystemWalletById()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletService.GetSystemWallet();

                return new ServiceResponse<WalletDTO>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddWallet(WalletDTO newWallet)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletService.AddWallet(newWallet);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{walletId:int}")]
        public async Task<IServiceResponse<object>> UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletService.TopUpWallet(walletId, walletTransactionDTO);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{walletId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWallet(int walletId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletService.RemoveWallet(walletId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("searchwallets")]
        public async Task<IServiceResponse<List<WalletDTO>>> SearchForWallets(WalletSearchOption searchOption)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletsObj = await _walletService.SearchForWallets(searchOption);

                return new ServiceResponse<List<WalletDTO>>
                {
                    Object = walletsObj
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("outstandingcorporatepayments")]
        public async Task<IServiceResponse<List<WalletDTO>>> GetOutstaningCorporatePayments()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletsObj = await _walletService.GetOutstaningCorporatePayments();

                return new ServiceResponse<List<WalletDTO>>
                {
                    Object = walletsObj
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("bulkcreditdebit")]
        public async Task<IServiceResponse<object>> BulkCreditDebit()
        {
            return await HandleApiOperationAsync(async () =>
            {
                object result = null;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    string mainFile = string.Empty;
                    foreach (string file in httpRequest.Files)
                    {
                        mainFile = file;
                        break;
                    }

                    var postedFile = httpRequest.Files[mainFile];
                    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    result = await _walletService.ProcessBulkWalletUpload(filePath);

                    //delete files after processing
                    File.Delete(filePath);
                }
                
                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

    }
}
