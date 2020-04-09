using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [RoutePrefix("api/rave")]
    public class FlutterwaveController : BaseWebApiController
    {
        private readonly IFlutterwavePaymentService _service;

        public FlutterwaveController(IFlutterwavePaymentService service) : base(nameof(FlutterwaveController))
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validatepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidatePayment(FlutterWebhookDTO webhookData)
        {
            //need to authenticate that the call is actually from flutterwave
            //Confirm the API key Yerima used to send when calling agility from firebase 
            //AIzaSyCl2wtzcjTd1ekKgpNNgQRNuqRjtM8qRic
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                response.Object = true;
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
