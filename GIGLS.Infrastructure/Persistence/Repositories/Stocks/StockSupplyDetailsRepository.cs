using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Stocks;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Stocks
{
    public class StockSupplyDetailsRepository : Repository<StockSupplyDetails, GIGLSContext>, IStockSupplyDetailsRepository
    {
        private GIGLSContext _context;

        public StockSupplyDetailsRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
