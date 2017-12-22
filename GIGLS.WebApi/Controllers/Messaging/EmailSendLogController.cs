using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.MessagingLog;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Messaging
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/emailsendlog")]
    public class EmailSendLogController : BaseWebApiController
    {
        private readonly IEmailSendLogService _messageService;

        public EmailSendLogController(IEmailSendLogService messageService) : base(nameof(EmailSendLogController))
        {
            _messageService = messageService;
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<EmailSendLogDTO>>> GetEmailSendLogs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var email = await _messageService.GetEmailSendLogAsync();

                return new ServiceResponse<IEnumerable<EmailSendLogDTO>>
                {
                    Object = email
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{messageId:int}")]
        public async Task<IServiceResponse<EmailSendLogDTO>> GetEmailSendLog(int messageId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var message = await _messageService.GetEmailSendLogById(messageId);

                return new ServiceResponse<EmailSendLogDTO>
                {
                    Object = message
                };
            });
        }
               
    }
}
