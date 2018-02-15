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
        
        public Task<List<StateDTO>> GetStatesAsync(int pageSize = 10, int page = 1)
        {
            var states = Context.State.AsQueryable();

            List<StateDTO> stateDto = (from s in states
                                       select new StateDTO
                                       {
                                           StateId = s.StateId,
                                           StateName = s.StateName,
                                           StateCode = s.StateCode,
                                           CountryId = s.CountryId,
                                           Country = Context.Country.Where(c => c.CountryId == s.CountryId).Select(x => new CountryDTO
                                           {
                                               CountryId = x.CountryId,
                                               CountryCode = x.CountryCode,
                                               CountryName = x.CountryName                                               
                                           }).FirstOrDefault(),
                                           DateCreated = s.DateCreated,
                                           DateModified = s.DateModified
                                       }).ToList();
            
            return Task.FromResult(stateDto);
        }

        public int GetStatesTotal()
        {
            var count = Context.State.ToList().Count();
            return count;
        }
        
        public Task<StateDTO> GetStateById(int stateId)
        {
            var states = Context.State.Where(x => x.StateId == stateId);

            StateDTO stateDto = (from s in states
                                 select new StateDTO
                                 {
                                     StateId = s.StateId,
                                     StateName = s.StateName,
                                     StateCode = s.StateCode,
                                     CountryId = s.CountryId,
                                     Country = Context.Country.Where(c => c.CountryId == s.CountryId).Select(x => new CountryDTO
                                     {
                                         CountryId = x.CountryId,
                                         CountryCode = x.CountryCode,
                                         CountryName = x.CountryName
                                     }).FirstOrDefault(),
                                     DateCreated = s.DateCreated,
                                     DateModified = s.DateModified
                                 }).FirstOrDefault();

            return Task.FromResult(stateDto);
        }
    }
}
