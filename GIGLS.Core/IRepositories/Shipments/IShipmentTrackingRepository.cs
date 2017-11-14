using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShipmentTrackingRepository : IRepository<ShipmentTracking>
    {
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync();
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync(string waybill);
        Task<List<ShipmentTrackingDTO>> GetShipmentWaitingForCollection();
    }
}
