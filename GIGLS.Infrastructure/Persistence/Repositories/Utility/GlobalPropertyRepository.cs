using POST.Core.Domain.Utility;
using POST.Core.IRepositories.Utility;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Utility
{
    public class GlobalPropertyRepository : Repository<GlobalProperty, GIGLSContext>, IGlobalPropertyRepository
    {
        public GlobalPropertyRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
