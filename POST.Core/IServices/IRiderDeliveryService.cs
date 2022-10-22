using POST.Core.DTO;
using POST.Core.DTO.Report;
using System.Threading.Tasks;

namespace POST.Core.IServices
{
    public interface IRiderDeliveryService : IServiceDependencyMarker
    {
        Task<RiderDeliveryViewDTO> GetRiderDelivery(string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria);
    }
}