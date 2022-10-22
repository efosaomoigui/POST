using POST.Core.DTO.SLA;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Sla
{
    public interface ISLAService : IServiceDependencyMarker
    {
        Task<IEnumerable<SLADTO>> GetSLAs();
        Task<SLADTO> GetSLAById(int id);
        Task<SLADTO> GetSLAByType(SLAType type);
        Task<object> AddSLA(SLADTO sla);
        Task UpdateSLA(int id, SLADTO sla);
        Task RemoveSLA(int id);
        Task<object> UserSignedSLA(int slaId);
    }
}
