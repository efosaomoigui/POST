using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
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

    }
}
