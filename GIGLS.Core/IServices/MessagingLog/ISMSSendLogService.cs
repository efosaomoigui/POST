using GIGLS.Core.DTO.MessagingLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.MessagingLog
{
    public interface ISmsSendLogService : IServiceDependencyMarker
    {
        Task<List<SmsSendLogDTO>> GetSmsSendLogAsync(MessageFilterOption filter);
        Task<SmsSendLogDTO> GetSmsSendLogById(int messageId);
        Task<object> AddSmsSendLog(SmsSendLogDTO messageDto);
        Task UpdateSmsSendLog(int messageId, SmsSendLogDTO messageDto);
        Task RemoveSmsSendLog(int messageId);
    }
}
