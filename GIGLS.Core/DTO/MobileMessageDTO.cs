using GIGLS.Core.Enums;

namespace GIGLS.Core.DTO
{
    public class MobileMessageDTO
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string WaybillNumber { get; set; }
        public string SenderPhoneNumber { get; set; }

        public int OTP { get; set; }
        public string ExpectedTimeofDelivery { get; set; }
        public SMSSenderPlatform SMSSenderPlatform { get; set; }

    }
}
