using POST.Core.Domain;
using POST.Core.IRepositories.Customers;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Customers
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
