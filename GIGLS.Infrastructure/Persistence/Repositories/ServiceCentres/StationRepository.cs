using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class StationRepository : Repository<Station, GIGLSContext>, IStationRepository
    {
        public StationRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<Station>> GetStationsAsync()
        {
            return Task.FromResult(Context.Station.Include("State").ToList());
        }
    }
}
