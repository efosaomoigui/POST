using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class WeightLimitRepository : Repository<WeightLimit, GIGLSContext>, IWeightLimitRepository
    {
        public WeightLimitRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
