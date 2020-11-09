using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO
{
    public class MessageDTO : BaseDomainDTO
    {
        public int MessageId { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ToEmail { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        public MessageType MessageType { get; set; }

        public string FinalBody { get; set; }

        public string CustomerName { get; set; }
        public string ReceiverName { get; set; }
        public string Waybill { get; set; }

        public string CustomerCode { get; set; }
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }


        public SMSSenderPlatform SMSSenderPlatform { get; set; }
    }

    public class NewMessageDTO : BaseDomainDTO
    {
        public string ReceiverDetail { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        public MessageType MessageType { get; set; }
    }
}
