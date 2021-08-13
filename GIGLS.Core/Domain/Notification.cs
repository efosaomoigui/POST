using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class Notification : BaseDomain
    {
        public int NotificationId { get; set; }
        [MaxLength(250)]
        public string Subject { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
        [MaxLength(500)]
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public MessageAction MesageActions { get; set; }
    }
}
