using POST.Core.DTO.OnlinePayment;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.PaymentWebhook
{
    [RoutePrefix("api/paystack")]
    [IPFilter]
    public class PaystackController : BaseWebApiController
    {
        private readonly IPaystackPaymentService _service;

        public PaystackController(IPaystackPaymentService service): base(nameof(PaystackController))
        {
            _service = service;
        }
        
        [HttpPost]
        [Route("validatepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidateWallet(PaystackWebhookDTO webhookData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _service.VerifyAndValidateWallet(webhookData);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        //Ghana Paystack call
        [HttpPost]
        [Route("processmobilepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidateMobilePayment(PaystackWebhookDTO webhookData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.VerifyAndValidateMobilePayment(webhookData);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}