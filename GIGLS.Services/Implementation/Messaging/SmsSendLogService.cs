using GIGLS.Core.IServices.MessagingLog;
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
    public class SmsSendLogService : ISmsSendLogService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public SmsSendLogService(IUserService userService, IUnitOfWork uow)
        {
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddSmsSendLog(SmsSendLogDTO messageDto)
        {
            var message = Mapper.Map<SmsSendLog>(messageDto);

            if (message.User == null)
            {
                message.User = await _userService.GetCurrentUserId();
            }
            _uow.SmsSendLog.Add(message);
            await _uow.CompleteAsync();
            return new { id = message.SmsSendLogId };
        }

        public Task<IEnumerable<SmsSendLogDTO>> GetSmsSendLogAsync()
        {
            var messages = _uow.SmsSendLog.GetSmsSendLogsAsync();
            return messages;
        }

        public async Task<SmsSendLogDTO> GetSmsSendLogById(int messageId)
        {
            var message = await _uow.SmsSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<SmsSendLogDTO>(message);
        }

        public async Task RemoveSmsSendLog(int messageId)
        {
            var message = await _uow.SmsSendLog.GetAsync(messageId);

            if (message == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            _uow.SmsSendLog.Remove(message);
            await _uow.CompleteAsync();
        }

        public async Task UpdateSmsSendLog(int messageId, SmsSendLogDTO messageDto)
        {
            var message = await _uow.SmsSendLog.GetAsync(messageId);

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
