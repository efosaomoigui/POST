using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IStateRepository : IRepository<State>
    {
        Task<List<StateDTO>> GetStatesAsync(int pageSize, int page);
        int GetStatesTotal();
        Task<StateDTO> GetStateById(int stateId);
        Task<IEnumerable<StateDTO>> GetStateByCountryId(int countryId);
    }
}
