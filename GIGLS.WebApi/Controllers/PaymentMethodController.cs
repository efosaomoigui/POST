using POST.Core.DTO;
using POST.Core.IServices;
using POST.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/paymentmethod")]
    public class PaymentMethodController : BaseWebApiController
    {
        private IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService) :base(nameof(PaymentMethodController))
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PaymentMethodNewDTO>>> PaymentMethods()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var methods = await _paymentMethodService.GetPaymentMethods();
                return new ServiceResponse<IEnumerable<PaymentMethodNewDTO>>
                {
                    Object = methods
                };
            });
        }

        [HttpGet]
        [Route("{paymentmethodId:int}")]
        public async Task<IServiceResponse<PaymentMethodNewDTO>> PaymentMethod(int paymentmethodId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var method = await _paymentMethodService.GetPaymentMethodById(paymentmethodId);
                return new ServiceResponse<PaymentMethodNewDTO>
                {
                    Object = method
                };
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddPaymentMethod(PaymentMethodNewDTO paymentMethodDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var method = await _paymentMethodService.AddPaymentMethod(paymentMethodDto);
                return new ServiceResponse<object>
                {
                    Object = method
                };
            });
        }

        [HttpPut]
        [Route("{paymentmethodId:int}")]
        public async Task<IServiceResponse<object>> UpdatePaymentMethod(int paymentmethodId, PaymentMethodNewDTO paymentMethodDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentMethodService.UpdatePaymentMethod(paymentmethodId, paymentMethodDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [HttpDelete]
        [Route("{paymentmethodId:int}")]
        public async Task<IServiceResponse<bool>> DeletePaymentMethod(int paymentmethodId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentMethodService.DeletePaymentMethod(paymentmethodId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPut]
        [Route("{paymentmethodId:int}/status/{status}")]
        public async Task<IServiceResponse<object>> UpdatePaymentMethodStatus(int paymentmethodId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentMethodService.UpdatePaymentMethodStatus(paymentmethodId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }
    }
}
