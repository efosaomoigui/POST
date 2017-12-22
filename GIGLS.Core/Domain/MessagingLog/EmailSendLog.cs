using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain.MessagingLog
{
    public class EmailSendLog : BaseDomain, IAuditable
    {
        public int EmailSendLogId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public MessagingLogStatus Status { get; set; }
        public string User { get; set; }
    }
}
