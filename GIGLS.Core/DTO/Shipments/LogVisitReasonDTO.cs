using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class LogVisitReasonDTO : BaseDomainDTO
    {
        public int LogVisitReasonId { get; set; }
        public string Message { get; set; }
    }
}