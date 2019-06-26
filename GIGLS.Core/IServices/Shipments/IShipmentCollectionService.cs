using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Shipments
{
    public interface IShipmentCollectionService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentCollections();
        Task<List<ShipmentCollectionDTO>> GetShipmentCollections(ShipmentCollectionFilterCriteria collectionFilterCriteria);
        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetShipmentCollections(FilterOptionsDto filterOptionsDto);
        Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentWaitingForCollection();
        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetShipmentWaitingForCollection(FilterOptionsDto filterOptionsDto);
        Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill);
        Task AddShipmentCollection(ShipmentCollectionDTO shipmentCollection);
        Task UpdateShipmentCollection(ShipmentCollectionDTO shipmentCollection);
        Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection);        
        Task RemoveShipmentCollection(string waybill);
        Task CheckShipmentCollection(string waybill);
        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetOverDueShipments(FilterOptionsDto filterOptionsDto);
        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetEcommerceOverDueShipments(FilterOptionsDto filterOptionsDto);

        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetOverDueShipmentsGLOBAL(FilterOptionsDto filterOptionsDto);
        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetEcommerceOverDueShipmentsGLOBAL(FilterOptionsDto filterOptionsDto);

        Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipmentsGLOBAL();
        Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipments();

        System.Tuple<Task<List<ShipmentCollectionDTO>>, int> GetShipmentWaitingForCollectionForHub(int pageIndex, int pageSize);

    }
}
