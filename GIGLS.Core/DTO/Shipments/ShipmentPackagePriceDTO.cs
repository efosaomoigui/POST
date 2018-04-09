using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentPackagePriceDTO : BaseDomainDTO
    {
        public int ShipmentPackagePriceId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
