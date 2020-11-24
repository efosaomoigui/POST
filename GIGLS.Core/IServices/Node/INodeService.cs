using GIGLS.Core.DTO.Node;

namespace GIGLS.Core.IServices.Node
{
    public interface INodeService : IServiceDependencyMarker
    {
        void WalletNotification(UserPayload user);
    }
}
