using GIGLS.Core.Enums;

namespace GIGLS.CORE.DTO.Shipments
{
    public class ShipmentCollectionDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string IndentificationUrl { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }

        //Who process the collection
        public string UserId { get; set; }
    }
}
