using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Haulage
{
    public class ZoneHaulagePriceDTO : BaseDomainDTO
    {
        public int ZoneHaulagePriceId { get; set; }
        
        public int HaulageId { get; set; }
        public decimal Tonne { get; set; }

        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public decimal Price { get; set; }
    }
}
