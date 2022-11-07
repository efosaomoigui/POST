using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.Domain.BankSettlement;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.Account;
using POST.Core.DTO.Wallet;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface ICashOnDeliveryAccountRepository : IRepository<CashOnDeliveryAccount>
    {
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync();
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync(CODStatus codStatus);
        IQueryable<CashOnDeliveryAccount> GetCODCollectedAsQueryable();
        Task<IEnumerable<NewInvoiceViewDTO>> GetCODCustomersWhoNeedPayOut();
        Task<Shipment> GetShipmentByWaybill(string waybill);
        Task<IQueryable<CodPayOutList>> GetPaidOutCODListsAsQueryable();
    }

}
