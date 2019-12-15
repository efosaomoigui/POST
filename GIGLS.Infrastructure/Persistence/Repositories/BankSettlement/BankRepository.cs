using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.BankSettlement
{
    public class BankRepository: Repository<Bank, GIGLSContext>, IBankRepository
    {
        public BankRepository(GIGLSContext context) : base(context)
        {

        }
    }
}
