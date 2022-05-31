using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class ShipmentCategory : Repository<InboundCategory, GIGLSContext>, IShipmentCategory
    {
        private GIGLSContext _context;
        public ShipmentCategory(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<InboundCategory>> GetInboundCategory()
        {
            try
            {
                var category = _context.InboundCategory;
                return Task.FromResult(category.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
