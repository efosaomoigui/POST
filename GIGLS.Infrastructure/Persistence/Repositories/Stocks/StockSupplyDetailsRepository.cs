using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Stocks;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Stocks
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
