using POST.Core.DTO.MessagingLog;
using POST.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.MessagingLog
{
    public interface IEmailSendLogService : IServiceDependencyMarker
    {
        Task<List<EmailSendLogDTO>> GetEmailSendLogAsync(MessageFilterOption filter);
        Task<Tuple<List<EmailSendLogDTO>, int>> GetEmailSendLogAsync(FilterOptionsDto filterOptionsDto);
        Task<EmailSendLogDTO> GetEmailSendLogById(int messageId);
        Task<object> AddEmailSendLog(EmailSendLogDTO messageDto);
        Task UpdateEmailSendLog(int messageId, EmailSendLogDTO messageDto);
        Task RemoveEmailSendLog(int messageId);
        Task<List<EmailSendLogDTO>> GetEmailSendLog(string email);
    }
}
