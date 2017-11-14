using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Fleets
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
