using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

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
        public string Waybill { get; set; }
    }
    public class SmsDeliveryDTO : BaseDomainDTO
    {
        public int reportcount { get; set; }
        public List<SmsDeliveryDataDTO> data { get; set; }
        
    }
    public class SmsDeliveryDataDTO
    {
        public string Mobile { get; set; }
        public string SentTime { get; set; }
        public string Done { get; set; }
        public string Status { get; set; }
        public string SMS { get; set; }

    }
}
