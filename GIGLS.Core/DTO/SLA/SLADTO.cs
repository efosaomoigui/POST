using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.SLA
{
    public class SLADTO : BaseDomainDTO
    {
        public int SLAId { get; set; }
        public string Content { get; set; }
        public SLAType SLAType { get; set; }
    }
}
