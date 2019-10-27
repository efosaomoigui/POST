using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IDeliveryLocationService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeliveryLocationDTO>> GetDeliveryLocations();
        Task<object> AddDeliveryLocationPrice(DeliveryLocationDTO deliveryLocationDTO);
        Task UpdateDeliveryLocationPrice(int deliveryLocationId, DeliveryLocationDTO deliveryLocationDTO);
        Task RemoveDeliveryLocationPrice(int deliveryLocationId);
        Task<DeliveryLocationDTO> GetDeliveryLocationById(int deliveryLocationId);

    }
}
