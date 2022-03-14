using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/paymentmethod")]
    public class PaymentMethodController : BaseWebApiController
    {
        private IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService) : base(nameof(PaymentMethodController))
        {
            _paymentMethodService = paymentMethodService;
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PaymentMethodNewDTO>>> GetPaymentMethods()
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{paymentMethodId:int}")]
        public async Task<IServiceResponse<PaymentMethodNewDTO>> GetPaymentMethodById(int paymentMethodId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var method = await _paymentMethodService.GetPaymentMethodById(paymentMethodId);
                return new ServiceResponse<PaymentMethodNewDTO>
                {
                    Object = method
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
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

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{paymentMethodId:int}")]
        public async Task<IServiceResponse<object>> UpdatePaymentMethod(int paymentMethodId, PaymentMethodNewDTO paymentMethodDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentMethodService.UpdatePaymentMethod(paymentMethodId, paymentMethodDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{paymentMethodId:int}")]
        public async Task<IServiceResponse<bool>> DeletePaymentMethod(int paymentMethodId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentMethodService.DeletePaymentMethod(paymentMethodId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{paymentMethodId:int}/status/{status}")]
        public async Task<IServiceResponse<object>> UpdateStatus(int paymentMethodId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _paymentMethodService.UpdatePaymentMethodStatus(paymentMethodId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

    }
}