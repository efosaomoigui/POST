using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.MessagingLog
{
    public class SmsSendLogDTO : BaseDomainDTO
    {
        public int SmsSendLogId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public MessagingLogStatus Status { get; set; }
        public string User { get; set; }
        public string ResultStatus { get; set; }
        public string ResultDescription { get; set; }
    }
}
