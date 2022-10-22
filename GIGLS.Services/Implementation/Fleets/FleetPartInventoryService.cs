using System;
using System.Threading.Tasks;
using POST.Core.DTO.Fleets;
using POST.Core.IServices.Fleets;

namespace POST.Services.Implementation.Fleets
{
    public class FleetPartInventoryService : IFleetPartInventoryService
    {
        public Task<object> AddFleetPart(FleetPartInventoryDTO inventory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFleetPartInventory(int inventoryId)
        {
            throw new NotImplementedException();
        }

        public Task<FleetPartInventoryDTO> GetFleetPartById(int inventoryId)
        {
            throw new NotImplementedException();
        }

        public Task<FleetPartInventoryDTO> GetFleetPartInventories()
        {
            throw new NotImplementedException();
        }

        public Task UpdateFleetPartInventory(int inventoryId, FleetPartInventoryDTO inventory)
        {
            throw new NotImplementedException();
        }
    }
}
