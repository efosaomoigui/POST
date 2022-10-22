using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface IDemurrageRegisterAccountRepository : IRepository<DemurrageRegisterAccount>
    {
         IQueryable<DemurrageRegisterAccount> GetDemurrageAsQueryable();
        //Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync();
    }
}
