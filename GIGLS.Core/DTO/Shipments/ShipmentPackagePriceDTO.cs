using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentPackagePriceDTO : BaseDomainDTO
    {
        public int ShipmentPackagePriceId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }
    }
}
