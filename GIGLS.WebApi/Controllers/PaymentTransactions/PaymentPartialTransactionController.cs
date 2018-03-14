using GIGLS.Core.IServices;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.PaymentTransactions
{
    [Authorize(Roles = "Account, ViewAdmin")]
    [RoutePrefix("api/paymentPartialTransaction")]
    public class PaymentPartialTransactionController : BaseWebApiController
    {
        private readonly IPaymentPartialTransactionService _paymentPartialService;

        public PaymentPartialTransactionController(IPaymentPartialTransactionService paymentPartialService) : base(nameof(PaymentPartialTransactionController))
        {
            _paymentPartialService = paymentPartialService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>> GetPaymentPartialTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _paymentPartialService.GetPaymentPartialTransactions();

                return new ServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>
                {
                    Object = payment
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Create")]
        //[HttpPost]
        //[Route("")]
        //public async Task<IServiceResponse<object>> AddPaymentTransaction(PaymentTransactionDTO paymentTransactionDto)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var payment = await _paymentService.AddPaymentTransaction(paymentTransactionDto);

        //        return new ServiceResponse<object>
        //        {
        //            Object = payment
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>> GetPaymentPartialTransactionByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _paymentPartialService.GetPaymentPartialTransactionById(waybill);

                return new ServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>
                {
                    Object = payment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeletePaymentPartialTransaction(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentPartialService.RemovePaymentPartialTransaction(waybill);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> UpdatePaymentPartialTransaction(string waybill, PaymentPartialTransactionDTO paymentPartialTransaction)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentPartialService.UpdatePaymentPartialTransaction(waybill, paymentPartialTransaction);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
