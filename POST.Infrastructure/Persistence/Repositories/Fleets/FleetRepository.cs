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
    public class FleetRepository : Repository<Fleet, GIGLSContext>, IFleetRepository
    {
        private GIGLSContext _context;

        public FleetRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<FleetDTO>> GetFleets()
        {
            try
            {
                var fleets = Context.Fleet.Include("FleetModel");

                var fleetDto = from r in fleets
                               select new FleetDTO
                               {
                                   FleetId = r.FleetId,
                                   ChassisNumber = r.ChassisNumber,
                                   EngineNumber = r.EngineNumber,
                                   RegistrationNumber = r.RegistrationNumber,
                                   Status = r.Status,
                                   Description = r.Description,
                                   FleetType = r.FleetType,
                                   ModelId = r.ModelId,
                                   ModelName = r.FleetModel.ModelName,
                                   Capacity = r.Capacity,
                                   PartnerId = r.PartnerId,                                 
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified
                               };
                return Task.FromResult(fleetDto.OrderBy(x => x.RegistrationNumber).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RegisterFleetsCaptainAsync()
        {

        }
    }
}
