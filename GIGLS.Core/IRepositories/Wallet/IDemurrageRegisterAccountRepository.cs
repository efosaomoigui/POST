using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface IDemurrageRegisterAccountRepository : IRepository<DemurrageRegisterAccount>
    {
         IQueryable<DemurrageRegisterAccount> GetDemurrageAsQueryable();
        //Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync();
    }
}
