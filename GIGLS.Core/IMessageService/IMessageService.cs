using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
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
        Task SendOverseasShipmentReceivedMails(ShipmentDTO shipmentDto);
        Task SendOverseasRequestMails(IntlShipmentRequestDTO shipmentDto, UserDTO user, string storeName);
        Task SendOverseasMails(MessageDTO messageDTO);
        Task SendOverseasPaymentConfirmationMails(ShipmentDTO shipmentDto);
    }
}