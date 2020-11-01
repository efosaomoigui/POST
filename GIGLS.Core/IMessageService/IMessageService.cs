using GIGLS.Core.DTO;
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
        Task<MessageDTO> GetMessageByType(MessageType messageType);
    }
}