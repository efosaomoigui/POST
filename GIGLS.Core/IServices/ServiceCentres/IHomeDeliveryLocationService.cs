using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IHomeDeliveryLocationService : IServiceDependencyMarker
    {
        Task<object> AddHomeDeliveryLocation(HomeDeliveryLocationDTO homeDeliveryLocationDTO);
        Task<HomeDeliveryLocationDTO> GetHomeDeliveryLocationById(int locationId);
        Task<IEnumerable<HomeDeliveryLocationDTO>> GetHomeDeliveryLocations();
        Task UpdateHomeDeliveryLocation(int locationId, HomeDeliveryLocationDTO homeDeliveryLocationDTO);
        Task UpdateHomeDeliveryLocation(int locationId, bool status);
        Task DeleteHomeDeliveryLocation(int locationId);
        Task<IEnumerable<HomeDeliveryLocationDTO>> GetActiveLocations();
    }
}
