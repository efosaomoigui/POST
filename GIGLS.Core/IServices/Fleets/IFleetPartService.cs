using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetPartService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetPartDTO>> GetFleetParts();
        Task<FleetPartDTO> GetFleetPartById(int partId);
        Task<object> AddFleetPart(int ModelId, FleetPartDTO part);
        Task UpdateFleetPart(int partId, FleetPartDTO part);
        Task DeleteFleetPart(int partId);
    }
}
