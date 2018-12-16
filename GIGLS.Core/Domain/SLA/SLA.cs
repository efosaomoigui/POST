using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain.SLA
{
    public class SLA : BaseDomain, IAuditable
    {
        public int SLAId { get; set; }
        public string Content { get; set; }
        public SLAType SLAType { get; set; }
    }
}
