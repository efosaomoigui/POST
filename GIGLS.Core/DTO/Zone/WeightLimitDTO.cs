using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class WeightLimitDTO : BaseDomainDTO
    {
        public int WeightLimitId { get; set; }
        public decimal Weight { get; set; }
    }
}
