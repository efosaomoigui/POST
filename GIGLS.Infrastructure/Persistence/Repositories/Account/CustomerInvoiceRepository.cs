using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class CustomerInvoiceRepository : Repository<CustomerInvoice, GIGLSContext>, ICustomerInvoiceRepository
    {
        private GIGLSContext _context;

        public CustomerInvoiceRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

    }
}