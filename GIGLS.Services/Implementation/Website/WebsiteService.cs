using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Website;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Website
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IMessageSenderService _messageSenderService;

        public WebsiteService(IMessageSenderService messageSenderService)
        {
            _messageSenderService = messageSenderService;
        }

        public async Task<bool> SendSchedulePickupMail (WebsiteMessageDTO obj)
        {
            try
            {
                var messageType = MessageType.WEBPICKUP;
                var emailSmsType = EmailSmsType.Email;
                obj.gigMail = "info@giglogistics.com";

                var result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SendQuoteMail(WebsiteMessageDTO obj)
        {
            try
            {
                var messageType = MessageType.WEBQUOTE;
                var emailSmsType = EmailSmsType.Email;
                obj.gigMail = "info@giglogistics.com";

                var result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SendGIGGoIssuesMail(AppMessageDTO obj)
        {
            try
            {
                var messageType = MessageType.APPREPORT;
                var emailSmsType = EmailSmsType.Email;
                obj.Recipient = "gopartners@giglogistics.ng";

                var result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
