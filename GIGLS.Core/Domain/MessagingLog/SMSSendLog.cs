using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.MessagingLog
{
    public class SmsSendLog : BaseDomain, IAuditable
    {
        public int SmsSendLogId { get; set; }

        [MaxLength(100)]
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public MessagingLogStatus Status { get; set; }

        [MaxLength(128)]
        public string User { get; set; }
        public string ResultStatus { get; set; }
        public string ResultDescription { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
    }
}