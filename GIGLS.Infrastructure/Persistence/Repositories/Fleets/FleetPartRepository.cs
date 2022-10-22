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
    public class FleetPartRepository : Repository<FleetPart, GIGLSContext>, IFleetPartRepository
    {
        private GIGLSContext _context;

        public FleetPartRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<FleetPartDTO>> GetFleetParts()
        {
            try
            {
                var vehicles = Context.FleetPart;

                var vehicleDto = from s in vehicles
                                 select new FleetPartDTO
                                 {
                                     PartName = s.PartName,
                                     PartId = s.PartId,
                                     ModelId = s.ModelId,
                                     ModelName = s.Model.ModelName,
                                     DateCreated = s.DateCreated,
                                     DateModified = s.DateModified
                                 };
                return Task.FromResult(vehicleDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
