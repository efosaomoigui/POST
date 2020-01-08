using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
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

        [HttpPost]
        [Route("processmobilepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidateMobilePayment(PaystackWebhookDTO webhookData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.ProcessMobilePayment(webhookData);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}