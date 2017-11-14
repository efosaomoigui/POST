using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetDTO>> GetFleets();
        Task<FleetDTO> GetFleetById(int fleetId);
        Task<object> AddFleet(FleetDTO fleet);
        Task UpdateFleet(int fleetId, FleetDTO fleet);
        Task DeleteFleet(int fleetId);               
        Task SetFleetCapacity(int fleetId, int capacity);
        Task<int> GetFleetCapacity(int fleetId);
        Task UpdateFleetStatus(int fleetId, bool status);
    }
}
