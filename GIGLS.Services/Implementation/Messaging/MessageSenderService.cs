using GIGLS.Core.Enums;
using GIGLS.Core.IMessage;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;

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

        public async Task<bool> SendMessage(MessageType messageType, EmailSmsType emailSmsType, object obj)
        {
            try
            {
                switch (emailSmsType)
                {
                    case EmailSmsType.Email:
                        {
                            await SendEmailMessage(messageType, obj);
                            break;
                        }
                    case EmailSmsType.SMS:
                        {
                            await SendSMSMessage(messageType, obj);
                            break;
                        }
                    case EmailSmsType.All:
                        {
                            await SendEmailMessage(messageType, obj);
                            await SendSMSMessage(messageType, obj);
                            break;
                        }
                }
            }
            catch (Exception)
            {
                //throw;
            }

            return await Task.FromResult(true);
        }

        private async Task SendEmailMessage(MessageType messageType, object obj)
        {
            try
            {
                var emailMessages = await _messageService.GetEmailAsync();
                var messageDTO = emailMessages.FirstOrDefault(s => s.MessageType == messageType);

                //prepare message finalBody
                PrepareMessageFinalBody(messageDTO, obj);
                await _emailService.SendAsync(messageDTO);
            }
            catch(Exception ex)
            {

            }
        }

        private async Task SendSMSMessage(MessageType messageType, object obj)
        {
            try
            {
                var smsMessages = await _messageService.GetSmsAsync();
                var messageDTO = smsMessages.FirstOrDefault(s => s.MessageType == messageType);

                //prepare message finalBody
                PrepareMessageFinalBody(messageDTO, obj);

                await _sMSService.SendAsync(messageDTO);
            }
            catch (Exception ex)
            {

            }
        }

        private void PrepareMessageFinalBody(MessageDTO messageDTO, object obj)
        {
            if(obj is ShipmentDTO)
            {
                var shipmentDTO = (ShipmentDTO)obj;
                messageDTO.FinalBody = string.Format(messageDTO.Body, shipmentDTO.Customer[0].CustomerName, shipmentDTO.Waybill);
                messageDTO.To = shipmentDTO.Customer[0].PhoneNumber;
            }

            //resolve phone numbers to +2347011111111
            var toPhoneNumber = messageDTO.To;
            //1
            if (toPhoneNumber.Trim().StartsWith("0"))   //07011111111
            {
                toPhoneNumber = toPhoneNumber.Substring(1, toPhoneNumber.Length - 1);
                toPhoneNumber = $"+234{toPhoneNumber}";
            }
            //2
            if (!toPhoneNumber.Trim().StartsWith("+"))  //2347011111111
            {
                toPhoneNumber = $"+{toPhoneNumber}";
            }

            //assign
            messageDTO.To = toPhoneNumber;
        }

    }
}
