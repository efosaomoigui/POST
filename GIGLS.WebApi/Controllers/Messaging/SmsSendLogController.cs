using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.MessagingLog;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Messaging
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/smssendlog")]
    public class SmsSendLogController : BaseWebApiController
    {
        private readonly ISmsSendLogService _messageService;

        public SmsSendLogController(ISmsSendLogService messageService) : base(nameof(MessageController))
        {
            _messageService = messageService;
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<SmsSendLogDTO>>> GetSmsSendLogs(MessageFilterOption filter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var sms = await _messageService.GetSmsSendLogAsync(filter);

                return new ServiceResponse<IEnumerable<SmsSendLogDTO>>
                {
                    Object = sms
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("search")]
        public async Task<IServiceResponse<IEnumerable<SmsSendLogDTO>>> GetSmsSendLogs([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var smsTuple = _messageService.GetSmsSendLogAsync(filterOptionsDto);
                return new ServiceResponse<IEnumerable<SmsSendLogDTO>>
                {
                    Object = await smsTuple.Item1,
                    Total = smsTuple.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{messageId:int}")]
        public async Task<IServiceResponse<SmsSendLogDTO>> GetSmsSendLog(int messageId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var message = await _messageService.GetSmsSendLogById(messageId);

                return new ServiceResponse<SmsSendLogDTO>
                {
                    Object = message
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{phoneNumber}/phonenumber")]
        public async Task<IServiceResponse<List<SmsSendLogDTO>>> GetSmsSendLog(string phoneNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var messages = await _messageService.GetSmsSendLog(phoneNumber);

                return new ServiceResponse<List<SmsSendLogDTO>>
                {
                    Object = messages
                };
            });
        }

    }
}
