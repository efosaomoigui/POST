using GIGLS.Core.DTO.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.BankSettlement
{
    public interface IBankShipmentSettlementService : IServiceDependencyMarker
    {
        Task<IEnumerable<InvoiceViewDTO>> GetCashShipmentSettlement();
    }

}
