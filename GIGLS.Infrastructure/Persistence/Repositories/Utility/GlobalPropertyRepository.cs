using GIGLS.Core.Domain.Utility;
using GIGLS.Core.IRepositories.Utility;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Utility
{
    public class GlobalPropertyRepository : Repository<GlobalProperty, GIGLSContext>, IGlobalPropertyRepository
    {
        public GlobalPropertyRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
