using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentCancelService : IServiceDependencyMarker
    {
        Task<List<ShipmentCancelDTO>> GetShipmentCancels();
        Task<List<ShipmentCancelDTO>> GetShipmentCancels(ShipmentCollectionFilterCriteria collectionFilterCriteria);
        Task<ShipmentCancelDTO> GetShipmentCancelById(string waybill);
        Task<object> AddShipmentCancel(string waybill,ShipmentCancelDTO shipmentCancelDTO);
    }
}
