using GIGLS.Core.DTO.Node;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Node
{
    public interface INodeService : IServiceDependencyMarker
    {
        Task WalletNotification(UserPayload user);
    }
}
