using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain
{
    public class EmailSms : BaseDomain, IAuditable
    {
        public int EmailSmsId { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        public MessageType MessageType { get; set; }
    }
}
