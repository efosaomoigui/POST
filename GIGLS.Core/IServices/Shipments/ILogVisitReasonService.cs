using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface ILogVisitReasonService : IServiceDependencyMarker
    {
        Task<List<LogVisitReasonDTO>> GetLogVisitReasons();
        Task<LogVisitReasonDTO> GetLogVisitReasonById(int logVisitReasonId);
        Task<object> AddLogVisitReason(LogVisitReasonDTO logVisitReasonDto);
        Task UpdateLogVisitReason(int logVisitReasonId, LogVisitReasonDTO logVisitReasonDto);
        Task DeleteLogVisitReason(int logVisitReasonId);
    }
}