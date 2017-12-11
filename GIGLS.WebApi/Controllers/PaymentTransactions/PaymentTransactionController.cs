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
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/paymenttransaction")]
    public class PaymentTransactionController : BaseWebApiController
    {
        private readonly IPaymentTransactionService _paymentService;

        public PaymentTransactionController(IPaymentTransactionService paymentService) : base(nameof(PaymentTransactionController))
        {
            _paymentService = paymentService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PaymentTransactionDTO>>> GetPaymentTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _paymentService.GetPaymentTransactions();

                return new ServiceResponse<IEnumerable<PaymentTransactionDTO>>
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
        public async Task<IServiceResponse<PaymentTransactionDTO>> GetPaymentTransactionByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _paymentService.GetPaymentTransactionById(waybill);

                return new ServiceResponse<PaymentTransactionDTO>
                {
                    Object = payment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeletePaymentTransaction(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentService.RemovePaymentTransaction(waybill);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> UpdateState(string waybill, PaymentTransactionDTO paymentTransaction)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentService.UpdatePaymentTransaction(waybill, paymentTransaction);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
