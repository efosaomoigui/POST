using POST.Core.DTO.Node;
using POST.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace POST.Core.IServices.Node
{
    public interface INodeService : IServiceDependencyMarker
    {
        Task WalletNotification(UserPayload user);
        Task CreateShipment(CreateShipmentNodeDTO nodePayload);
        Task<NewNodeResponse> RemoveShipmentFromQueue(string waybill);
        Task<AcceptShipmentResponse> AssignShipmentToPartner(AcceptShipmentPayload nodePayload);
        Task<NewNodeResponse> RemovePendingShipment(PendingNodeShipmentDTO dto);
        Task<NewNodeResponse> UpdateMerchantSubscription(UpdateNodeMercantSubscriptionDTO dto);
        Task<NewNodeResponse> UpdateMerchantDetails(UpdateNodeMercantDetailsDTO dto);
        Task<NewNodeResponse> PushNotificationsToEnterpriseAPI(PushNotificationMessageDTO dto);
    }
}
