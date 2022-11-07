using POST.Core.Enums;
using POST.CORE.DTO;

namespace POST.Core.DTO.SLA
{
    public class SLADTO : BaseDomainDTO
    {
        public int SLAId { get; set; }
        public string Content { get; set; }
        public SLAType SLAType { get; set; }
        public bool IsSigned { get; set; }
    }
}
