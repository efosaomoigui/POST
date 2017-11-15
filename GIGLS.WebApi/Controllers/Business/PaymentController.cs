using GIGLS.Core.IServices;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Business;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Business
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
        [Route("{waybill}/cash")]
        public async Task<IServiceResponse<bool>> CashPayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            return await HandleApiOperationAsync(async () =>
           {
               var cash = await _paymentService.ProcessCashPayment(waybill, paymentDto);

               return new ServiceResponse<bool>
               {
                   Object = cash
               };
           });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("{waybill}/pos")]
        public async Task<IServiceResponse<bool>> PosPayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var pos = await _paymentService.ProcessPosPayment(waybill, paymentDto);

                return new ServiceResponse<bool>
                {
                    Object = pos
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("{waybill}/online")]
        public async Task<IServiceResponse<bool>> OnlinePayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var online = await _paymentService.ProcessOnlinePayment(waybill, paymentDto);

                return new ServiceResponse<bool>
                {
                    Object = false
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("{waybill}/confirmpayment")]
        public async Task<IServiceResponse<bool>> ConfirmPayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _paymentService.ConfirmPayment(waybill, paymentDto);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

    }
}
