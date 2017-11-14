using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Stocks;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Stocks
{
    public class StockRequestPartRepository : Repository<StockRequestPart, GIGLSContext>, IStockRequestPartRepository
    {
        private GIGLSContext _context;

        public StockRequestPartRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
