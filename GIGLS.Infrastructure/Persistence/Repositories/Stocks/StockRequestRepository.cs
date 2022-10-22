using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Stocks;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Stocks
{
    public class StockRequestRepository : Repository<StockRequest, GIGLSContext>, IStockRequestRepository
    {
        private GIGLSContext _context;

        public StockRequestRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
