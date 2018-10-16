using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO
{
    public class NotificationDTO : BaseDomainDTO
    {
        public int NotificationId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
