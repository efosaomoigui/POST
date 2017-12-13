using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface ICashOnDeliveryAccountRepository : IRepository<CashOnDeliveryAccount>
    {
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync();
    }
}
