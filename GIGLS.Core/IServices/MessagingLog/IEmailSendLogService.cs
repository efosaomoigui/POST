using GIGLS.Core.DTO.MessagingLog;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.MessagingLog
{
    public interface IEmailSendLogService : IServiceDependencyMarker
    {
        Task<List<EmailSendLogDTO>> GetEmailSendLogAsync(MessageFilterOption filter);
        System.Tuple<Task<List<EmailSendLogDTO>>, int> GetEmailSendLogAsync(FilterOptionsDto filterOptionsDto);
        Task<EmailSendLogDTO> GetEmailSendLogById(int messageId);
        Task<object> AddEmailSendLog(EmailSendLogDTO messageDto);
        Task UpdateEmailSendLog(int messageId, EmailSendLogDTO messageDto);
        Task RemoveEmailSendLog(int messageId);
    }
}
