using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using System.Linq;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class DispatchActivityRepository : Repository<DispatchActivity, GIGLSContext>, IDispatchActivityRepository
    {
        private GIGLSContext _context;

        public DispatchActivityRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<DispatchActivityDTO>> GetDispatchActivitiesAsync()
        {
            try
            {
                var dispatchActivitys = Context.DispatchActivity;

                var dispatchActivityDto = from r in dispatchActivitys
                                  select new DispatchActivityDTO
                                  {
                                      DispatchActivityId = r.DispatchActivityId,
                                      DispatchId = r.DispatchId,
                                      Description = r.Description,
                                      Location = r.Location,
                                      DateCreated = r.DateCreated,
                                      DateModified = r.DateModified
                                  };
                return Task.FromResult(dispatchActivityDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
