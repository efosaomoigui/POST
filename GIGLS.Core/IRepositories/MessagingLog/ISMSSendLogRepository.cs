using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.MessagingLog;
using GIGLS.Core.DTO.MessagingLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.MessagingLog
{
    public interface ISmsSendLogRepository : IRepository<SmsSendLog>
    {
        Task<List<SmsSendLogDTO>> GetSmsSendLogsAsync(MessageFilterOption filter);
    }
}
