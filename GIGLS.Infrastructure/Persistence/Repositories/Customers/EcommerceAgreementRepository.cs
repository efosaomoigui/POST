using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Customers;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Customers
{
    public class EcommerceAgreementRepository : Repository<EcommerceAgreement, GIGLSContext>, IEcommerceAgreementRepository
    {
        private GIGLSContext _context;

        public EcommerceAgreementRepository(GIGLSContext context): base(context)
        {
            _context = context;
        }
    }
}
