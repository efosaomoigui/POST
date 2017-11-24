using GIGLS.Core.Enums;
using GIGLS.Core.IMessage;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Messaging
{
    public class MessageSenderService : IMessageSenderService
    {
        private IEmailService _emailService;
        private ISMSService _sMSService;
        private IMessageService _messageService;

        public MessageSenderService(IEmailService emailService, ISMSService sMSService,
            IMessageService messageService)
        {
            _emailService = emailService;
            _sMSService = sMSService;
            _messageService = messageService;
        }

        public async Task<bool> SendMessage(MessageType messageType, EmailSmsType emailSmsType)
        {
            try
            {
                switch (emailSmsType)
                {
                    case EmailSmsType.Email:
                        {
                            await SendEmailMessage(messageType);
                            break;
                        }
                    case EmailSmsType.SMS:
                        {
                            await SendSMSMessage(messageType);
                            break;
                        }
                    case EmailSmsType.All:
                        {
                            await SendEmailMessage(messageType);
                            await SendSMSMessage(messageType);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }

            return await Task.FromResult(true);
        }

        private async Task SendEmailMessage(MessageType messageType)
        {
            var emailMessages = await _messageService.GetEmailAsync();
            var messageDTO = emailMessages.FirstOrDefault(s => s.MessageType == messageType);
            await _emailService.SendAsync(messageDTO);
        }

        private async Task SendSMSMessage(MessageType messageType)
        {
            var smsMessages = await _messageService.GetSmsAsync();
            var messageDTO = smsMessages.FirstOrDefault(s => s.MessageType == messageType);
            await _sMSService.SendAsync(messageDTO);
        }
    }
}
