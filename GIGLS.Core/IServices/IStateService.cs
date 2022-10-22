using POST.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices
{
    public interface IStateService : IServiceDependencyMarker
    {
        Task<IEnumerable<StateDTO>> GetStates(int pageSize=10, int page=1);
        Task<StateDTO> GetStateById(int stateId);
        Task<object> AddState(StateDTO state);
        Task UpdateState(int stateId, StateDTO state);
        Task RemoveState(int stateId);
        int GetStatesTotal();
        Task<IEnumerable<StateDTO>> GetStatesByUserActiveCountryId();
    }
}
