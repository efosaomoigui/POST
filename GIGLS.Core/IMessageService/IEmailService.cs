using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessage
{
    public interface IEmailService : IServiceDependencyMarker
    {
        Task<string> SendAsync(MessageDTO message);
        Task<string> SendEcommerceRegistrationNotificationAsync(MessageDTO message);
        Task<string> SendPaymentNotificationAsync(MessageDTO message);
        Task<string> SendCustomerRegistrationMails(MessageDTO message);
        Task<string> SendOverseasShipmentMails(MessageDTO message);
        Task<string> SendEmailIntlShipmentAsync(MessageDTO message);
        Task<string> SendEmailShipmentARFAsync(MessageDTO message);
        Task<string> SendEmailEcommerceCustomerRepAsync(MessageDTO message);
        Task<string> SendEmailShipmentCreationAsync(MessageDTO message);
        Task<string> SendEmailShipmentARFHomeDeliveryAsync(MessageDTO message);
        Task<string> SendEmailShipmentARFTerminalPickupAsync(MessageDTO message);
        Task<string> SendEmailClassCustomerShipmentCreationAsync(MessageDTO message);
        Task<string> SendConfigCorporateSignUpMessage(MessageDTO message);
        Task<string> SendConfigCorporateNubanAccMessage(MessageDTO message);
        Task<string> SendEmailForReceivedItem(MessageDTO message);
    }
}
