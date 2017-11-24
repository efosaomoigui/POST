using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IMessageService : IServiceDependencyMarker
    {
        Task<IEnumerable<MessageDTO>> GetSmsAsync();
        Task<IEnumerable<MessageDTO>> GetEmailAsync();
        Task<MessageDTO> GetMessageById(int messageId);
        Task<object> AddMessage(MessageDTO messageDto);
        Task UpdateMessage(int messageId, MessageDTO messageDto);
        Task RemoveMessage(int messageId);
    }
}
