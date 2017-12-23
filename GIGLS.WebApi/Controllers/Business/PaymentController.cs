using GIGLS.Core.IServices;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Business;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Business
{
    [Authorize(Roles = "Shipment")]
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
        [Route("ProcessPayment")]
        public async Task<IServiceResponse<bool>> ProcessPayment(PaymentTransactionDTO paymentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _paymentService.ProcessPayment(paymentDto);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

    }
}
