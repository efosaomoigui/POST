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
