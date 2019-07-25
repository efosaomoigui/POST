using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class WeightPercentDTO : BaseDomainDTO
    {
        public string Category { get; set; }
        public string PriceType { get; set; }
        public string CustomerType { get; set; }
        public string ModificationType { get; set; }
        public string RateType { get; set; }
        public decimal WeightOne { get; set; }
        public decimal WeightTwo { get; set; }
        public decimal WeightThree { get; set; }
    }
}