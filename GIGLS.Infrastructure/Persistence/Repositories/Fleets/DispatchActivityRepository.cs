using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class DispatchActivityRepository : Repository<DispatchActivity, GIGLSContext>, IDispatchActivityRepository
    {
        public DispatchActivityRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<DispatchActivityDTO>> GetDispatchActivitiesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
