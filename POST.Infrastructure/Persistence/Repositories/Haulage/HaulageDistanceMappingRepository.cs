using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Haulage;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Haulage
{
    public class HaulageDistanceMappingRepository : Repository<HaulageDistanceMapping, GIGLSContext>, IHaulageDistanceMappingRepository
    {
        public HaulageDistanceMappingRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
