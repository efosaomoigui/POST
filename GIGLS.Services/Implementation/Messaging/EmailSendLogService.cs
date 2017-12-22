using GIGLS.Core.IServices.MessagingLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core;
using GIGLS.Core.Domain.MessagingLog;
using AutoMapper;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Messaging
{
    public class EmailSendLogService : IEmailSendLogService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public EmailSendLogService(IUserService userService, IUnitOfWork uow)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }
        public async Task<object> AddEmailSendLog(EmailSendLogDTO messageDto)
        {
            var message = Mapper.Map<EmailSendLog>(messageDto);
            if (message.User == null)
            {
                message.User = await _userService.GetCurrentUserId();
            }
            _uow.EmailSendLog.Add(message);
            await _uow.CompleteAsync();
            return new { id = message.EmailSendLogId };
        }

        public Task<List<EmailSendLogDTO>> GetEmailSendLogAsync(MessageFilterOption filter)
        {
            var messages = _uow.EmailSendLog.GetEmailSendLogsAsync(filter);
            return messages;
        }

        public async Task<EmailSendLogDTO> GetEmailSendLogById(int messageId)
        {
            var message = await _uow.EmailSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<EmailSendLogDTO>(message);
        }

        public async Task RemoveEmailSendLog(int messageId)
        {
            var message = await _uow.EmailSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            _uow.EmailSendLog.Remove(message);
            await _uow.CompleteAsync();
        }

        public async Task UpdateEmailSendLog(int messageId, EmailSendLogDTO messageDto)
        {
            var message = await _uow.EmailSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            
            message.From = messageDto.From;
            message.To = messageDto.To;
            message.Status = messageDto.Status;
            message.Message = messageDto.Message;
            message.User = messageDto.User;
            
            await _uow.CompleteAsync();
        }
    }
}
