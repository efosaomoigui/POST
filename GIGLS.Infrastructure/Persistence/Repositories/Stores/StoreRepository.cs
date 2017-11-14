using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;


namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Stores
{
    public class StoreRepository : Repository<Store, GIGLSContext>, IStoreRepository
    {
        private GIGLSContext _context;

        public StoreRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
