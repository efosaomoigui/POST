using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Shipments
{
    public interface IShipmentCollectionService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentCollections();
        Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentWaitingForCollection();
        Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill);
        Task AddShipmentCollection(ShipmentCollectionDTO shipmentCollection);
        Task UpdateShipmentCollection(ShipmentCollectionDTO shipmentCollection);
        Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection);        
        Task RemoveShipmentCollection(string waybill);
        Task CheckShipmentCollection(string waybill);
    }
}
