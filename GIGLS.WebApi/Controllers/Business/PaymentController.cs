using POST.Core.IServices;
using POST.Core.DTO.PaymentTransactions;
using POST.Core.IServices.Business;
using POST.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using POST.WebApi.Filters;

namespace POST.WebApi.Controllers.Business
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("ProcessPartialPayment")]
        public async Task<IServiceResponse<bool>> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _paymentService.ProcessPaymentPartial(paymentPartialTransactionProcessDTO);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }
    }
}
