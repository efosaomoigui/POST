using GIGL.POST.Core.Domain;
using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Shipments
{
    public interface IShipmentCancelService : IServiceDependencyMarker
    {
        Task<List<ShipmentCancelDTO>> GetShipmentCancels();
        Task<List<ShipmentCancelDTO>> GetShipmentCancels(ShipmentCollectionFilterCriteria collectionFilterCriteria);
        Task<ShipmentCancelDTO> GetShipmentCancelById(string waybill);
        Task<object> AddShipmentCancel(string waybill,ShipmentCancelDTO shipmentCancelDTO);
        Task<object> ProcessShipmentCancel(Shipment shipment, string userId, string cancelReason);
    }
}
