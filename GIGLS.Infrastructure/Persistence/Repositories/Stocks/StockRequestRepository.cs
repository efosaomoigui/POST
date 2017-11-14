using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Stocks;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Stocks
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
