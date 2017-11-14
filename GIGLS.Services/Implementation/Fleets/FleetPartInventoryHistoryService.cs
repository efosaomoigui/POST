using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices.Fleets;

namespace GIGLS.Services.Implementation.Fleets
{
    public class FleetPartInventoryHistoryService : IFleetPartInventoryHistoryService
    {
        public Task<object> AddFleetPartInventoryHistory(FleetPartInventoryHistoryDTO history)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFleetPartInventoryHistory(int inventoryId)
        {
            throw new NotImplementedException();
        }

        public Task<FleetPartInventoryHistoryDTO> GetFleetPartById(int inventoryId)
        {
            throw new NotImplementedException();
        }

        public Task<FleetPartInventoryHistoryDTO> GetFleetPartInventories()
        {
            throw new NotImplementedException();
        }

        public Task UpdateFleetPartInventoryHistory(int inventoryId, FleetPartInventoryHistoryDTO history)
        {
            throw new NotImplementedException();
        }
    }
}
