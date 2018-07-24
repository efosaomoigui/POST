using GIGLS.Core.DTO.Zone;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentDeliveryOptionMappingDTO : BaseDomainDTO
    {
        public int ShipmentDeliveryOptionMappingId { get; set; }
        
        public string Waybill { get; set; }
        
        public int DeliveryOptionId { get; set; }
        public DeliveryOptionDTO DeliveryOption { get; set; }
    }
}
