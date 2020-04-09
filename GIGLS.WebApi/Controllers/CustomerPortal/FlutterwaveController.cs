using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
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

        [HttpPost]
        [Route("validatepayment")]
        public async Task<IServiceResponse<bool>> VerifyAndValidatePayment(FlutterWebhookDTO webhookData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //need to authenticate that the call is actually from flutterwave

                await _service.VerifyAndValidatePayment(webhookData);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
