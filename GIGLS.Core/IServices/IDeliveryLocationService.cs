using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IDeliveryLocationService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeliveryLocationDTO>> GetDeliveryLocations();
    }
}
