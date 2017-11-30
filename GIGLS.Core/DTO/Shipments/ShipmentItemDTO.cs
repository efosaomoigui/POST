using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentItemDTO : BaseDomainDTO
    {
        public int ShipmentItemId { get; set; }
        public string Description { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public double Weight { get; set; }
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; }

        //Foreign key information
        public int ShipmentId { get; set; }

    }
}
