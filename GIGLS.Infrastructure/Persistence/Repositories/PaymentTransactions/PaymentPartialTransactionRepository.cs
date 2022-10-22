using POST.Core.Domain;
using POST.Core.IRepositories.PaymentTransactions;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.PaymentTransactions
{
    public class PaymentPartialTransactionRepository : Repository<PaymentPartialTransaction, GIGLSContext>, IPaymentPartialTransactionRepository
    {
        public PaymentPartialTransactionRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
