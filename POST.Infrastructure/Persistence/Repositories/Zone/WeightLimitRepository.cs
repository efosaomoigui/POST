using POST.Core.Domain;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class WeightLimitRepository : Repository<WeightLimit, GIGLSContext>, IWeightLimitRepository
    {
        public WeightLimitRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
