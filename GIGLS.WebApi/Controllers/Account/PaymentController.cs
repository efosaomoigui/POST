using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Account;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Account
{
    [Authorize]
    [RoutePrefix("api/payment")]
    public class PaymentController : BaseWebApiController
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService) : base(nameof(PaymentController))
        {
            _paymentService = paymentService;
        }


        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> ConfirmPayment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _paymentService.ConfirmPayment(waybill);

                return new ServiceResponse<object>
                {
                    Object = payment
                };
            });
        }

    }
}
