using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IRiderDeliveryService : IServiceDependencyMarker
    {
        Task<RiderDeliveryViewDTO> GetRiderDelivery(string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria);
    }
}