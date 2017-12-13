using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Fleets
{
    public interface IDispatchRepository : IRepository<Dispatch>
    {
        Task<IEnumerable<DispatchDTO>> GetDispatchAsync();
    }
}
