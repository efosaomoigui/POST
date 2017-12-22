using GIGLS.Core.DTO.MessagingLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.MessagingLog
{
    public interface IEmailSendLogService : IServiceDependencyMarker
    {
        Task<List<EmailSendLogDTO>> GetEmailSendLogAsync(MessageFilterOption filter);
        Task<EmailSendLogDTO> GetEmailSendLogById(int messageId);
        Task<object> AddEmailSendLog(EmailSendLogDTO messageDto);
        Task UpdateEmailSendLog(int messageId, EmailSendLogDTO messageDto);
        Task RemoveEmailSendLog(int messageId);
    }
}
