using POST.CORE.DTO;

namespace POST.Core.DTO.Zone
{
    public class WeightLimitDTO : BaseDomainDTO
    {
        public int WeightLimitId { get; set; }
        public decimal Weight { get; set; }
        public bool Status { get; set; }
    }
}
