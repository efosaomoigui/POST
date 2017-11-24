using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessage
{
    public interface ISMSService : IServiceDependencyMarker
    {
        Task SendAsync(MessageDTO message);
    }
}
