using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Haulage;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Haulage
{
    public class HaulageDistanceMappingRepository : Repository<HaulageDistanceMapping, GIGLSContext>, IHaulageDistanceMappingRepository
    {
        public HaulageDistanceMappingRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
