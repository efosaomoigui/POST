using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
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

        public Task<IEnumerable<StateDTO>> GetStateByCountryId(int countryId)
        {
            var states = Context.State.Where(x => x.CountryId == countryId);

            IEnumerable<StateDTO> stateDto = (from s in states
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
                                 }).AsEnumerable();

            return Task.FromResult(stateDto);
        }
    }
}
