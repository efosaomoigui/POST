using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
{
    public class PartnerTransactionsRepository : Repository<PartnerTransactions, GIGLSContext>, IPartnerTransactionsRepository
    {

        public PartnerTransactionsRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
