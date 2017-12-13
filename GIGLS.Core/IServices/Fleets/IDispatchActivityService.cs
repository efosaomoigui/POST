using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IDispatchActivityService : IServiceDependencyMarker
    {
        Task<IEnumerable<DispatchActivityDTO>> GetDispatchActivities();
        Task<DispatchActivityDTO> GetDispatchActivityById(int DispatchActivityId);
        Task<object> AddDispatchActivity(DispatchActivityDTO DispatchActivity);
        Task UpdateDispatchActivity(int DispatchActivityId, DispatchActivityDTO DispatchActivity);
        Task DeleteDispatchActivity(int DispatchActivityId);
    }
}
