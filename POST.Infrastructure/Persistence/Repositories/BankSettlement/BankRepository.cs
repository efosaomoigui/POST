using POST.Core.Domain;
using POST.Core.IRepositories.BankSettlement;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.BankSettlement
{
    public class BankRepository: Repository<Bank, GIGLSContext>, IBankRepository
    {
        public BankRepository(GIGLSContext context) : base(context)
        {

        }
    }
}
