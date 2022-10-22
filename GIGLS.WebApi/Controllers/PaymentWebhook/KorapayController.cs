using POST.Core.DTO.OnlinePayment;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Infrastructure;
using POST.Services.Implementation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.PaymentWebhook
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
        public async Task<IHttpActionResult> VerifyAndValidatePayment(KorapayWebhookDTO webhookData)
        {
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
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
            return Ok();
        }

        //For Test no api key
        [AllowAnonymous]
        [HttpPost]
        [Route("validatepayment2")]
        public async Task<IHttpActionResult> VerifyAndValidatePayment2(KorapayWebhookDTO webhookData)
        {
            var request = Request;
            var headers = request.Headers;
            if (headers.Contains("x-korapay-signature"))
            {
                var key = await _korapayService.Encrypt(webhookData);
                string token = headers.GetValues("x-korapay-signature").FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    await _korapayService.VerifyAndValidatePaymentForWebhook(webhookData);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
            return Ok();
        }
    }
}