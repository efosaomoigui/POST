using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.PaymentWebhook
{
    [RoutePrefix("api/ussd")]
    public class USSDController : BaseWebApiController
    {
        private readonly IUssdService _ussdService;

        public USSDController(IUssdService ussdService) : base(nameof(USSDController))
        {
            _ussdService = ussdService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validatepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidatePayment(USSDWebhook webhook)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>
                {
                    Object = false
                };

                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("token") && headers.Contains("publickey"))
                {
                    string publicKey = _ussdService.GetPublicKey();
                    string token = _ussdService.GetToken();

                    string headerToken = headers.GetValues("token").FirstOrDefault();
                    string headerPublicKey = headers.GetValues("publickey").FirstOrDefault();

                    if (headerToken == token && headerPublicKey == publicKey)
                    {
                        await _ussdService.VerifyAndValidatePayment(webhook.Transaction_Ref);
                        response.Object = true;
                    }
                }
                return response;
            });
        }
    }
}