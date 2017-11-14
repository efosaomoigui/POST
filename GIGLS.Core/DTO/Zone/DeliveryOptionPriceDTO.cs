using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class DeliveryOptionPriceDTO : BaseDomainDTO
    {
        public int DeliveryOptionPriceId { get; set; }

        public int ZoneId { get; set; }
        public string ZoneName { get; set; }

        public int DeliveryOptionId { get; set; }
        public string DeliveryOption { get; set; }

        public decimal Price { get; set; }
    }
}
