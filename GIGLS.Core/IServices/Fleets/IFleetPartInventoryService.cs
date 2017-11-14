using GIGLS.Core.DTO.Fleets;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetPartInventoryService : IServiceDependencyMarker
    {
        Task<FleetPartInventoryDTO> GetFleetPartInventories();
        Task<FleetPartInventoryDTO> GetFleetPartById(int inventoryId);
        Task<object> AddFleetPart(FleetPartInventoryDTO inventory);
        Task UpdateFleetPartInventory(int inventoryId, FleetPartInventoryDTO inventory);
        Task DeleteFleetPartInventory(int inventoryId);
    }
}
