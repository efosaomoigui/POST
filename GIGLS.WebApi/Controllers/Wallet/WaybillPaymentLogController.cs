using POST.Core.DTO.OnlinePayment;
using POST.Core.DTO.Wallet;
using POST.Core.IServices;
using POST.Core.IServices.Wallet;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "Account, Shipment")]
    [RoutePrefix("api/waybillpaymentlog")]
    public class WaybillPaymentLogController : BaseWebApiController
    {
        private readonly IWaybillPaymentLogService _waybillPaymentLogService;

        public WaybillPaymentLogController(IWaybillPaymentLogService waybillPaymentLogService) : base(nameof(WaybillPaymentLogController))
        {
            _waybillPaymentLogService = waybillPaymentLogService;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<PaystackWebhookDTO>> AddWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLogDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybillPayment = await _waybillPaymentLogService.AddWaybillPaymentLog(waybillPaymentLogDTO);

                return new ServiceResponse<PaystackWebhookDTO>
                {
                    Object = waybillPayment
                };
            });
        }

        [HttpGet]
        [Route("verifypayment/{waybill}")]
        public async Task<IServiceResponse<PaystackWebhookDTO>> VerifyAndValidateWaybill([FromUri]  string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _waybillPaymentLogService.VerifyAndValidateWaybill(waybill);

                return new ServiceResponse<PaystackWebhookDTO>
                {
                    Object = result
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("verifypayment2/{waybill}")]
        public async Task<IServiceResponse<PaystackWebhookDTO>> VerifyAndValidateWaybill2([FromUri] string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _waybillPaymentLogService.VerifyAndValidateWaybill(waybill);

                return new ServiceResponse<PaystackWebhookDTO>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("processpayment/{waybill}/{pin}")]
        public async Task<IServiceResponse<PaystackWebhookDTO>> VerifyAndValidatePaymentUsingOTP([FromUri]  string waybill, [FromUri]  string pin)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //var result = await _waybillPaymentLogService.VerifyAndValidateWaybillForVodafoneMobilePayment(waybill, pin);
                var result = await _waybillPaymentLogService.VerifyAndValidatePaymentUsingOTP(waybill, pin);

                return new ServiceResponse<PaystackWebhookDTO>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<List<WaybillPaymentLogDTO>>> GetWaybillPaymentLogs([FromUri]  string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _waybillPaymentLogService.GetWaybillPaymentLogListByWaybill(waybill);

                return new ServiceResponse<List<WaybillPaymentLogDTO>>
                {
                    Object = result
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getallwaybillsforfailedpayments")]
        public async Task<IServiceResponse<List<WaybillPaymentLogDTO>>> GetAllWaybillsForFailedPayments() 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _waybillPaymentLogService.GetAllWaybillsForFailedPayments();

                return new ServiceResponse<List<WaybillPaymentLogDTO>>
                {
                    Object = result 
                };
            });
        }

        [HttpGet]
        [Route("gatewaycode")]
        public async Task<IServiceResponse<GatewayCodeResponse>> GetGatewayCode()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _waybillPaymentLogService.GetGatewayCode();
                return new ServiceResponse<GatewayCodeResponse>
                {
                    Object = result
                };
            });
        }
    }
}
