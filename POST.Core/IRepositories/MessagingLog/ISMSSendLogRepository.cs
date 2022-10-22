using GIGL.POST.Core.Repositories;
using POST.Core.Domain.MessagingLog;
using POST.Core.DTO.MessagingLog;
using POST.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.MessagingLog
{
    public interface ISmsSendLogRepository : IRepository<SmsSendLog>
    {
        Task<List<SmsSendLogDTO>> GetSmsSendLogsAsync(MessageFilterOption filter);
        Tuple<Task<List<SmsSendLogDTO>>, int> GetSmsSendLogsAsync(FilterOptionsDto filterOptionsDto);
    }
}
