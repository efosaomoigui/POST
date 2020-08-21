using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Stores
{
    public class ShipmentPackagingTransactionsRepository : Repository<ShipmentPackagingTransactions, GIGLSContext>, IShipmentPackagingTransactionsRepository
    {
        private GIGLSContext _context;

        public ShipmentPackagingTransactionsRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
