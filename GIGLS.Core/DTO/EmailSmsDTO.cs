using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO
{
    public class EmailSmsDTO : BaseDomainDTO
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
