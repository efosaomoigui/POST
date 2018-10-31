using GIGLS.Core.Enums;

namespace GIGLS.CORE.DTO.Shipments
{
    public class OverdueShipmentDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }

        public OverdueShipmentStatus OverdueShipmentStatus { get; set; }

        //Who processed the collection
        public string UserId { get; set; }
    }
}
