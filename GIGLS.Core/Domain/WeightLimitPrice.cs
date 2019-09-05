using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain
{
    public class WeightLimitPrice : BaseDomain, IAuditable
    {
        public int WeightLimitPriceId { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        
        public RegularEcommerceType RegularEcommerceType { get; set; }

        public int CountryId { get; set; }
    }
}
