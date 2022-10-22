using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Shipments
{
    public interface IShipmentDeliveryOptionMappingService : IServiceDependencyMarker
    {
        Task<List<ShipmentDeliveryOptionMappingDTO>> GetAllShipmentDeliveryOptionMappings();
        Task<List<ShipmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill);
        Task<object> AddShipmentDeliveryOptionMapping(ShipmentDeliveryOptionMappingDTO mapping);
    }
}
