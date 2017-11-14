using GIGLS.Core.DTO.Fleets;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetPartInventoryHistoryService : IServiceDependencyMarker
    {
        Task<FleetPartInventoryHistoryDTO> GetFleetPartInventories();
        Task<FleetPartInventoryHistoryDTO> GetFleetPartById(int inventoryId);
        Task<object> AddFleetPartInventoryHistory(FleetPartInventoryHistoryDTO history);
        Task UpdateFleetPartInventoryHistory(int inventoryId, FleetPartInventoryHistoryDTO history);
        Task DeleteFleetPartInventoryHistory(int inventoryId);
    }
}
