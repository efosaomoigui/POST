using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessageService
{
    public interface IMessageSenderService : IServiceDependencyMarker
    {
        Task<bool> SendMessage(MessageType messageType, EmailSmsType emailSmsType, object obj);
        Task SendGenericEmailMessage(MessageType messageType, object obj);
        Task SendVoiceMessageAsync(string userId);
        Task SendEcommerceRegistrationNotificationAsync(MessageDTO messageDTO);
        Task SendPaymentNotificationAsync(MessageDTO messageDTO);
        Task<MessageDTO> GetMessageByType(MessageType messageType, int countryId);
        Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType);
        Task SendCustomerRegistrationMails(MessageDTO messageDTO);
        Task SendOverseasShipmentReceivedMails(ShipmentDTO shipmentDto, List<string> generalPaymentLinks, int? isInNigeria);
        Task SendOverseasRequestMails(IntlShipmentRequestDTO shipmentDto, UserDTO user, string storeName);
        Task SendOverseasMails(MessageDTO messageDTO);
        Task SendOverseasPaymentConfirmationMails(ShipmentDTO shipmentDto);
        Task SendGeneralMailPayment(ShipmentDTO shipmentDto, List<string> generalPaymentLinks);
        Task SendMailsToIntlShipmentSender(MessageDTO messageDTO);
        Task SendMailsShipmentARF(MessageDTO messageDTO);
        Task SendMailsEcommerceCustomerRep(MessageDTO messageDTO);
        Task SendMailsShipmentCreation(MessageDTO messageDTO);
        Task SendMailsShipmentARFHomeDelivery(MessageDTO messageDTO);
        Task SendMailsShipmentARFTerminalPickup(MessageDTO messageDTO);
        Task<bool> SendEmailToCustomerForShipmentCreation(ShipmentDTO shipment);
        Task SendMailsClassCustomerShipmentCreation(MessageDTO messageDTO);
        //Task SendWhatsappMessage(ShipmentDTO shipmentDto);
        Task<string> SendWhatsappMessage(WhatsAppMessageDTO whatsappMessage);
        Task<string> ManageOptInOutForWhatsappNumber(WhatsappNumberDTO whatsappNumber);
        Task<string> SendWhatsappMessageTemporal(MessageType messageType, object tracking);
        Task SendConfigCorporateSignUpMessage(MessageDTO messageDTO);
        Task SendConfigCorporateNubanAccMessage(MessageDTO messageDTO);
        Task SendEmailForReceivedItem(MessageDTO messageDTO);
        Task SendShipmentRegisteredWithGigGoMails(IntlShipmentRequestDTO shipmentDto);
        Task SendShipmentRequestConfirmation(IntlShipmentRequestDTO shipmentDto);
        Task SendEmailForService(MessageDTO messageDTO);
        Task SendEmailForStellaLoginDetails(MessageDTO messageDTO);
        Task SendEmailFleetDisputeMessageAsync(MessageDTO messageDTO);
        Task SendEmailOpenJobCardAsync(MessageDTO messageDTO);
        Task SendEmailCloseJobCardAsync(MessageDTO messageDTO);
    }
}