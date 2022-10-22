using POST.Core.DTO.OnlinePayment;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Services.Implementation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.PaymentWebhook
{
    [RoutePrefix("api/sterling")]
    public class SterlingController : BaseWebApiController
    {
        private readonly ISterlingPaymentService _service;

        public SterlingController(ISterlingPaymentService service) : base(nameof(SterlingController))
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validatepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidatePayment(FlutterWebhookDTO webhookData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>
                {
                    Object = true
                };
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("verif-hash"))
                {
                    var key = await _service.GetSecurityKey();
                    string token = headers.GetValues("verif-hash").FirstOrDefault();
                    if (token == key)
                    {
                        await _service.VerifyAndValidatePayment(webhookData);
                    }
                }
                return response;
            });
        }
    }
}
