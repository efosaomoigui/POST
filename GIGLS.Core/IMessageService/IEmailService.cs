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
    }
}
