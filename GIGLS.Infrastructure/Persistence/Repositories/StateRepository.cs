using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
{
    public class StateRepository : Repository<State, GIGLSContext>, IStateRepository
    {
        public StateRepository(GIGLSContext context) : base(context)
        {
        }
        public Task<IEnumerable<StateDTO>> GetStatesAsync(int pageSize = 10, int page = 1)
        {
            var states = Context.State.ToList();
            var subresult  = states.Skip(pageSize * (page - 1)).Take(pageSize);
            var stateDto = Mapper.Map<IEnumerable<StateDTO>>(subresult);
            return Task.FromResult(stateDto);
        }

        public int GetStatesTotal()
        {
            var count = Context.State.ToList().Count();
            return count;
        }
    }
}
