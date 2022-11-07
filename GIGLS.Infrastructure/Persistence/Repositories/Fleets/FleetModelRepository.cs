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
    public class FleetModelRepository : Repository<FleetModel, GIGLSContext>, IFleetModelRepository
    {
        private GIGLSContext _context;

        public FleetModelRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<FleetModelDTO>> GetFleetModels()
        {
            try
            {
                var models = Context.FleetModel.Include("FleetMake");

                var modelDto = from r in models
                               select new FleetModelDTO
                               {
                                   ModelId = r.ModelId,
                                   ModelName = r.ModelName,
                                   MakeId = r.MakeId,
                                   MakeName = r.FleetMake.MakeName,
                                   DateCreated = r.DateCreated,
                                   DateModified = r.DateModified
                               };
                return Task.FromResult(modelDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
