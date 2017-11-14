using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IStateRepository : IRepository<State>
    {
        Task<IEnumerable<StateDTO>> GetStatesAsync(int pageSize, int page);
        int GetStatesTotal();
    }
}
