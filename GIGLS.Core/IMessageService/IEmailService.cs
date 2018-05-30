using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessage
{
    public interface IEmailService : IServiceDependencyMarker
    {
        Task<string> SendAsync(MessageDTO message);
    }
}
