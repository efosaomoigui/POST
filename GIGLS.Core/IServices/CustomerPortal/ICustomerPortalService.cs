using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CustomerPortal
{
    public interface ICustomerPortalService : IServiceDependencyMarker
    {
        Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria);
        Task<WalletTransactionSummaryDTO> GetWalletTransactions();
        Task<IEnumerable<InvoiceViewDTO>> GetInvoices();
        Task<InvoiceDTO> GetInvoiceByWaybill(string waybill);
        Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber);
        Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount();

    }
}
