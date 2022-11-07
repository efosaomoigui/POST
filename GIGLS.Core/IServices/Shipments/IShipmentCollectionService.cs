using POST.Core.DTO;
using POST.Core.DTO.Report;
using POST.Core.IServices;
using POST.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IServices.Shipments
{
    public interface IShipmentCollectionService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentCollections();
        Task<List<ShipmentCollectionDTO>> GetShipmentCollections(ShipmentCollectionFilterCriteria collectionFilterCriteria);
        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetShipmentCollections(FilterOptionsDto filterOptionsDto);
        Task<IEnumerable<ShipmentCollectionDTO>> GetShipmentWaitingForCollection();
        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetShipmentWaitingForCollection(FilterOptionsDto filterOptionsDto);
        Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill);
        Task AddShipmentCollection(ShipmentCollectionDTO shipmentCollection);
        Task UpdateShipmentCollection(ShipmentCollectionDTO shipmentCollection);
        Task UpdateShipmentCollectionForReturn(ShipmentCollectionDTO shipmentCollection);
        Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection);
        Task ReleaseShipmentForCollectionOnScanner(ShipmentCollectionDTO shipmentCollection);
        Task RemoveShipmentCollection(string waybill);
        Task CheckShipmentCollection(string waybill);
        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetOverDueShipments(FilterOptionsDto filterOptionsDto);
        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetEcommerceOverDueShipments(FilterOptionsDto filterOptionsDto);

        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetOverDueShipmentsGLOBAL(FilterOptionsDto filterOptionsDto);
        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetEcommerceOverDueShipmentsGLOBAL(FilterOptionsDto filterOptionsDto);

        Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipmentsGLOBAL();
        Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipments();

        Task<Tuple<List<ShipmentCollectionDTO>, int>> GetShipmentWaitingForCollectionForHub(FilterOptionsDto filterOptionsDto);

        Task AddRiderToDeliveryTable(ShipmentCollectionDTO shipmentCollection);
        Task<IEnumerable<ShipmentCollectionDTO>> GetEcommerceOverDueShipmentsGLOBAL(ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<ShipmentCollectionForContactDTO>> GetShipmentsCollectionForContact(ShipmentContactFilterCriteria baseFilterCriteria);
        Task<List<ShipmentCollectionDTOForArrived>> GetArrivedShipmentCollection(ShipmentContactFilterCriteria baseFilterCriteria);
        Task<GenerateAccountDTO> GenerateAccountNumberCellulant(GenerateAccountPayloadDTO payload);
        Task<CODPaymentResponse> GetTransferStatus(string craccount);
    }
}
