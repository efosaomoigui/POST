using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.BankSettlement
{
    public class GIGXUserDetailRepository : Repository<GIGXUserDetail, GIGLSContext>, IGIGXUserDetailRepository
    {
        public GIGXUserDetailRepository(GIGLSContext context) : base(context)
        {

        }
    }
}
