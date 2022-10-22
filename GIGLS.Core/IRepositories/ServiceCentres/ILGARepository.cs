using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.ServiceCentres
{
    public interface ILGARepository : IRepository<LGA>
    {
        Task<IEnumerable<LGADTO>> GetActiveLGAs();
        Task<IEnumerable<LGADTO>> GetActiveHomeDeliveryLocations();
    }
}
