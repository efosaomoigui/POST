using GIGL.POST.Core.Repositories;
using POST.Core.Domain.MessagingLog;
using POST.Core.DTO.MessagingLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.MessagingLog
{
    public interface IEmailSendLogRepository : IRepository<EmailSendLog>
    {
        Task<List<EmailSendLogDTO>> GetEmailSendLogsAsync(MessageFilterOption filter);
    }
}
