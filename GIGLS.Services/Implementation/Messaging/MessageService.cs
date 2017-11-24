using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Messaging
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _uow;

        public MessageService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public Task<IEnumerable<MessageDTO>> GetEmailAsync()
        {
            var emails = _uow.Message.GetMessageAsync(EmailSmsType.Email);
            return emails;
        }
        public Task<IEnumerable<MessageDTO>> GetSmsAsync()
        {
            var sms = _uow.Message.GetMessageAsync(EmailSmsType.SMS);
            return sms;
        }

        public async Task<object> AddMessage(MessageDTO messageDto)
        {
            var message = Mapper.Map<Message>(messageDto);
            _uow.Message.Add(message);
            await _uow.CompleteAsync();
            return new { id = message.MessageId };
        }

        public async Task<MessageDTO> GetMessageById(int messageId)
        {
            var message = await _uow.Message.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<MessageDTO>(message);
        }


        public async Task RemoveMessage(int messageId)
        {
            var message = await _uow.Message.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            _uow.Message.Remove(message);
            await _uow.CompleteAsync();
        }

        public async Task UpdateMessage(int messageId, MessageDTO messageDto)
        {
            var message = await _uow.Message.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }

            message.Body = messageDto.Body;
            message.Subject = messageDto.Subject;
            message.From = messageDto.From;
            message.To = messageDto.To;
            message.EmailSmsType = messageDto.EmailSmsType;
            message.MessageType = messageDto.MessageType;
            await _uow.CompleteAsync();
        }
    }
}
