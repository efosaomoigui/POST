using POST.Core.Domain;
using POST.Core.IRepositories.PaymentTransactions;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.PaymentTransactions
{
    public class PaymentTransactionRepository : Repository<PaymentTransaction, GIGLSContext>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(GIGLSContext context) : base(context)
        {
        }        
    }
}
