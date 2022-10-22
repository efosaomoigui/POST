using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Fleets;
using POST.Core.IRepositories.Fleets;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Fleets
{
    public class FleetMakeRepository : Repository<FleetMake, GIGLSContext>, IFleetMakeRepository
    {
        private GIGLSContext _context;

        public FleetMakeRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<FleetMakeDTO>> GetFleetMakers()
        {
            try
            {
                var makers = Context.FleetMake;

                var makerDto = from r in makers
                                 select new FleetMakeDTO
                                 {
                                     MakeId = r.MakeId,
                                     MakeName = r.MakeName,
                                     DateCreated = r.DateCreated,
                                     DateModified = r.DateModified
                                 };
                return Task.FromResult(makerDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
