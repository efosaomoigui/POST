using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface ICashOnDeliveryRegisterAccountRepository : IRepository<CashOnDeliveryRegisterAccount>
    {
        IQueryable<CashOnDeliveryRegisterAccount> GetCODAsQueryable();
        //Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync();
    }
}
