using System;

namespace GIGLS.Core.Domain
{
    public class WeightLimitPrice : BaseDomain, IAuditable
    {
        public int WeightLimitPriceId { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        public decimal Price { get; set; }
        public decimal Weight { get; set; }
    }
}
