using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.PaymentTransactions;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.PaymentTransactions
{
    public class PaymentPartialTransactionRepository : Repository<PaymentPartialTransaction, GIGLSContext>, IPaymentPartialTransactionRepository
    {
        public PaymentPartialTransactionRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
