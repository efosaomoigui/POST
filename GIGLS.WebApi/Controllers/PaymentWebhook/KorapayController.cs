using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.PaymentWebhook
{
    //[Authorize]
    [RoutePrefix("api/korapayment")]
    public class KorapaymentController : BaseWebApiController
    {
        // GET: Korapay
        private readonly IKorapayPaymentService _korapayService;

        public KorapaymentController(IKorapayPaymentService korapayService) : base(nameof(KorapaymentController))
        {
            _korapayService = korapayService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validatepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidatePayment(KorapayWebhookDTO webhookData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>
                {
                    Object = true
                };
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("x-korapay-signature"))
                {
                    var key = await _korapayService.Encrypt(webhookData);
                    string token = headers.GetValues("x-korapay-signature").FirstOrDefault();
                    if (token == key)
                    {
                        await _korapayService.VerifyAndValidatePaymentForWebhook(webhookData);
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }
    }
}