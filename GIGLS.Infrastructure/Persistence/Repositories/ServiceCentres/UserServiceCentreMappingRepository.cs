using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class UserServiceCentreMappingRepository : Repository<UserServiceCentreMapping, GIGLSContext>, IUserServiceCentreMappingRepository
    {
        private GIGLSContext _context;
        public UserServiceCentreMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
