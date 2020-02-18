using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
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

        //process payment for vodafone
        [HttpGet]
        [Route("processpayment/{waybill}/{pin}")]
        public async Task<IServiceResponse<PaystackWebhookDTO>> VerifyAndValidateWaybillForVodafoneMobilePayment([FromUri]  string waybill, [FromUri]  string pin)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _waybillPaymentLogService.VerifyAndValidateWaybillForVodafoneMobilePayment(waybill, pin);

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
    }
}
