using GIGLS.Core.IServices;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers
{
    //[Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/message")]
    public class MessageController : BaseWebApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService):base(nameof(MessageController))
        {
            _messageService = messageService;
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("email")]
        public async Task<IServiceResponse<IEnumerable<MessageDTO>>> GetEmails()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var emails = await _messageService.GetEmailAsync();

                return new ServiceResponse<IEnumerable<MessageDTO>>
                {
                    Object = emails
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("sms")]
        public async Task<IServiceResponse<IEnumerable<MessageDTO>>> GetSms()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var sms = await _messageService.GetSmsAsync();

                return new ServiceResponse<IEnumerable<MessageDTO>>
                {
                    Object = sms
                };
            });
        }


        //[GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddMessage(MessageDTO messageDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var message = await _messageService.AddMessage(messageDto);

                return new ServiceResponse<object>
                {
                    Object = message
                };
            });
        }

       // [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{messageId:int}")]
        public async Task<IServiceResponse<MessageDTO>> GetMessage(int messageId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var message = await _messageService.GetMessageById(messageId);

                return new ServiceResponse<MessageDTO>
                {
                    Object = message
                };
            });
        }

       // [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{messageId:int}")]
        public async Task<IServiceResponse<bool>> DeleteMessage(int messageId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _messageService.RemoveMessage(messageId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{messageId:int}")]
        public async Task<IServiceResponse<bool>> UpdateMessage(int messageId, MessageDTO messageDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _messageService.UpdateMessage(messageId, messageDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
