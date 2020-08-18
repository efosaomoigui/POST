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

    public class MobileShipmentCreationMessageDTO
    {
        public string SenderName { get; set; }
        public string WaybillNumber { get; set; }
        public string CustomerCarePhoneNumber1 { get; set; }
        public string CustomerCarePhoneNumber2 { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string GroupCode { get; set; }
        public SMSSenderPlatform SMSSenderPlatform { get; set; }

    }

    public class ShipmentDeliveryDelayMessageDTO
    {
        public string SenderName { get; set; }
        public string WaybillNumber { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string StationName { get; set; }
        public SMSSenderPlatform SMSSenderPlatform { get; set; }
    }

    public class ShipmentCancelMessageDTO
    {
        public string SenderName { get; set; }
        public string WaybillNumber { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string SenderEmail { get; set; }
        public string Reason { get; set; }
        public SMSSenderPlatform SMSSenderPlatform { get; set; }
    }
}
