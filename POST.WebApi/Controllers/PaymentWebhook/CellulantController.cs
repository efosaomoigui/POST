using POST.Core.DTO;
using POST.Core.DTO.OnlinePayment;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.CORE.DTO.Report;
using POST.Services.Implementation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POST.WebApi.Controllers.PaymentWebhook
{
    [Authorize]
    [RoutePrefix("api/cellulant")]
    public class CellulantController : BaseWebApiController
    {
        private readonly ICellulantPaymentService _cellulantService;

        public CellulantController(ICellulantPaymentService cellulantService) : base(nameof(CellulantController))
        {
            _cellulantService = cellulantService;
        }

        [HttpPost]
        [Route("transferDetails")]
        public async Task<IServiceResponse<List<TransferDetailsDTO>>> GetTransferDetailsLogFilter(BaseFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cellulantService.GetTransferDetails(filterCriteria);

                return new ServiceResponse<List<TransferDetailsDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("searchtransferdetails")]
        public async Task<IServiceResponse<List<TransferDetailsDTO>>> GetTransferDetailsLogFilterByAccountNumber(BaseFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cellulantService.GetTransferDetailsByAccountNumber(filterCriteria);

                return new ServiceResponse<List<TransferDetailsDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("checkoutencryption")]
        public async Task<IServiceResponse<CellulantResponseDTO>> CheckoutEncrption(CellulantPayloadDTO payload)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cellulantService.CheckoutEncryption(payload);

                return new ServiceResponse<CellulantResponseDTO>
                {
                    Object = result
                };
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("verifypayment")]
        public async Task<CellulantPaymentResponse> VerifyAndValidatePayment(CellulantWebhookDTO webhook)
        {
               return await _cellulantService.VerifyAndValidatePaymentForWebhook(webhook);
        }
    }
}

