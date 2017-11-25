using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain
{
    public class Message : BaseDomain, IAuditable
    {
        public int MessageId { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        public MessageType MessageType { get; set; }
    }
}
