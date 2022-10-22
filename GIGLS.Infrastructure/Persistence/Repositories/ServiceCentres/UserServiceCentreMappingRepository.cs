using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.ServiceCentres;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
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
