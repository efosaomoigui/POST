using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface ICashOnDeliveryBalanceRepository : IRepository<CashOnDeliveryBalance>
    {
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalanceAsync();
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryPaymentSheetAsync();
    }
}
