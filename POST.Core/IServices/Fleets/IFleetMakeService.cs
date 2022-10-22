using POST.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Fleets
{
    public interface IFleetMakeService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetMakeDTO>> GetFleetMakes();
        Task<FleetMakeDTO> GetFleetMakeById(int makeId);
        Task<object> AddFleetMake(FleetMakeDTO make);
        Task UpdateFleetMake(int makeId, FleetMakeDTO make);
        Task DeleteFleetMake(int makeId);
    }
}
