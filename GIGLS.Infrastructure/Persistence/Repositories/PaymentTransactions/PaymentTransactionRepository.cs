using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.PaymentTransactions;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.PaymentTransactions
{
    public class PaymentTransactionRepository : Repository<PaymentTransaction, GIGLSContext>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(GIGLSContext context) : base(context)
        {
        }        
    }
}
