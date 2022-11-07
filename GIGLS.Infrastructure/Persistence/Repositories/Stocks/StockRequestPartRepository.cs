using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Stocks;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Stocks
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
