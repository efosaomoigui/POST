using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ServiceCentres
{
    public interface IHomeDeliveryLocationRepository : IRepository<HomeDeliveryLocation>
    {
        Task<IEnumerable<HomeDeliveryLocationDTO>> GetActiveHomeDeliveryLocations();
    }
    
}
