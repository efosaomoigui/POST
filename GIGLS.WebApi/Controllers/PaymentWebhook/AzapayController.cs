using POST.Core.DTO;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.PaymentWebhook
{
    [Authorize]
    [RoutePrefix("api/azapay")]
    public class AzapayController : BaseWebApiController
    {
        private readonly IAzapayPaymentService _azapayService;
        public AzapayController(IAzapayPaymentService azapayService) : base(nameof(AzapayController))
        {
            _azapayService = azapayService;
        }

        [HttpGet]
        [Route("validateaccount/{accountnumber}")]
        public async Task<IServiceResponse<ValidateTimedAccountResponseDTO>> ValidateTimedAccount(string accountnumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _azapayService.ValidateTimedAccountRequest(accountnumber);

                return new ServiceResponse<ValidateTimedAccountResponseDTO>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("gettransactionhistory")]
        public async Task<IServiceResponse<GetTransactionHistoryResponseDTO>> GetTransactionHistory()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _azapayService.GetTransactionHistoryRequest();

                return new ServiceResponse<GetTransactionHistoryResponseDTO>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("initiatetimedaccount")]
        public async Task<IServiceResponse<InitiateTimedAccountResponseDTO>> InitiateTimedAccount(InitiateTimedAccountRequestDTO request)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _azapayService.InitiateTimedAccountRequest(request);

                return new ServiceResponse<InitiateTimedAccountResponseDTO>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("azapaytransfer")]
        public async Task<IServiceResponse<AzapayTransferResponseDTO>> AzapayTransfer(AzapayTransferRequestDTO request)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _azapayService.AzapayTransferRequest(request);

                return new ServiceResponse<AzapayTransferResponseDTO>
                {
                    Object = result
                };
            });
        }
    }
}
