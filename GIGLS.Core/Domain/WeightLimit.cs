using System;

namespace GIGLS.Core.Domain
{
    public class WeightLimit : BaseDomain, IAuditable
    {
        public int WeightLimitId { get; set; }
        public decimal Weight { get; set; }
        public bool Status { get; set; }
    }
}
