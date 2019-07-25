using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class WeightPercentDTO : BaseDomainDTO
    {
        public string Category { get; set; }
        public PricingType PriceType { get; set; }
        public PartnerType CustomerType { get; set; }
        public ModificationType ModificationType { get; set; }
        public RateType RateType { get; set; }
        public decimal WeightOne { get; set; }
        public decimal WeightTwo { get; set; }
        public decimal WeightThree { get; set; }
    }
}