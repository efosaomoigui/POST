using GIGLS.Core.DTO.Node;
using GIGLS.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Node
{
    public interface INodeService : IServiceDependencyMarker
    {
        Task WalletNotification(UserPayload user);
        Task CreateShipment(CreateShipmentNodeDTO nodePayload);
    }
}
