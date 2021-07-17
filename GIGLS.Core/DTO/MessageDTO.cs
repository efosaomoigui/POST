using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GIGLS.Core.DTO
{
    public class MessageDTO : BaseDomainDTO
    {
        public MessageDTO()
        {
            Emails = new List<string>();
        }
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
        public List<string> Emails { get; set; }
        public string MessageTemplate { get; set; } //Message Template to use
        public IntlMessageDTO IntlMessage { get; set; }
        public string Country { get; set; }

        public SMSSenderPlatform SMSSenderPlatform { get; set; }
        public IntlShipmentMessageDTO IntlShipmentMessage { get; set; }
        public EcommerceCustomerRepMessageDTO EcommerceMessage { get; set; }
        public ShipmentCreationMessageDTO ShipmentCreationMessage { get; set; }
    }

    public class NewMessageDTO
    {
        public string ReceiverDetail { get; set; }
        public EmailSmsType EmailSmsType { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }

    public class IntlMessageDTO
    {
        public string Description { get; set; }
        public string DepartureCenter { get; set; }
        public string DestinationCenter { get; set; }
        public string DeliveryOption { get; set; }
        public string RequestCode { get; set; }
        public string StoreOfPurchase { get; set; }
        public string ShippingCost { get; set; }
        public string DiscountedShippingCost { get; set; }
        public string PaymentLink { get; set; }
        public string DeliveryAddressOrCenterName { get; set; }
        public string DeliveryCode { get; set; }
        public string GeneralPaymentLinkI { get; set; }
        public string GeneralPaymentLinkII { get; set; }
    }

    public class IntlShipmentMessageDTO
    {
        public string Description { get; set; }
        public string DestinationCountry { get; set; }
        public string DepartureCenter { get; set; }
        public string DestinationCenter { get; set; }
    }

    public class EcommerceCustomerRepMessageDTO
    {
        public string AccountOfficerName { get; set; }
        public string AccountOfficerNumber { get; set; }
        public string AccountOfficerEmail { get; set; }
    }

    public class ShipmentCreationMessageDTO
    {
        public string DeliveryNumber { get; set; }
        public string ShippingCost { get; set; }
        public string DiscountedShippingCost { get; set; }
    }

    public class TypeTextDTO
    {
        [JsonProperty("preview_url")]
        public bool PreviewUrl { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class WhatsAppMessageDTO
    {
        public WhatsAppMessageDTO()
        {
            TypeText = new List<TypeTextDTO>();
        }
        [JsonProperty("recipient_whatsapp")]
        public string RecipientWhatsapp { get; set; }
        [JsonProperty("recipient_type")]
        public string RecipientType { get; set; }
        [JsonProperty("message_type")]
        public string MessageType { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("x-apiheader")]
        public string XApiheader { get; set; }
        [JsonProperty("type_text")]
        public List<TypeTextDTO> TypeText { get; set; }
        public string Waybill { get; set; }
    }

    public class WhatsAppMessagesDTO
    {
        public WhatsAppMessagesDTO()
        {
            Message = new List<WhatsAppMessageDTO>();
        }
        [JsonProperty("message")]
        public List<WhatsAppMessageDTO> Message { get; set; }
    }

    public class WhatsappNumberDTO
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class RecipientDTO
    {
        [JsonProperty("recipient")]
        public long Recipient { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }
    }

    public class ManageWhatsappConsentDTO
    {
        public ManageWhatsappConsentDTO()
        {
            Recipients = new List<RecipientDTO>();
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("recipients")]
        public List<RecipientDTO> Recipients { get; set; }
    }
}
