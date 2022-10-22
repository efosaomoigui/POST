using POST.Core.DTO.Zone;
using POST.CORE.DTO;

namespace POST.Core.DTO.Shipments
{
    public class ShipmentDeliveryOptionMappingDTO : BaseDomainDTO
    {
        public int ShipmentDeliveryOptionMappingId { get; set; }
        
        public string Waybill { get; set; }
        
        public int DeliveryOptionId { get; set; }
        public DeliveryOptionDTO DeliveryOption { get; set; }
    }
}
