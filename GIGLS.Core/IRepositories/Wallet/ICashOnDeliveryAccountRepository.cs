using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface ICashOnDeliveryAccountRepository : IRepository<CashOnDeliveryAccount>
    {
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync();
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountAsync(CODStatus codStatus);
        IQueryable<CashOnDeliveryAccount> GetCODCollectedAsQueryable();
        Task<IEnumerable<InvoiceViewDTO>> GetCODCustomersWhoNeedPayOut();
        Task<Shipment> GetShipmentByWaybill(string waybill);
    }

}
