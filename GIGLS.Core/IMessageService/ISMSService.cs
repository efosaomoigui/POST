using POST.Core.DTO;
using POST.Core.IServices;
using System.Threading.Tasks;

namespace POST.Core.IMessage
{
    public interface ISMSService : IServiceDependencyMarker
    {
        Task<string> SendAsync(MessageDTO message);
        Task SendVoiceMessageAsync(string phoneNumber);
    }
}
