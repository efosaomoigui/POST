using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class DispatchRepository : Repository<Dispatch, GIGLSContext>, IDispatchRepository
    {
        public DispatchRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<DispatchDTO>> GetDispatchAsync()
        {
            throw new NotImplementedException();
        }
    }
}
