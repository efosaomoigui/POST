using GIGLS.Core.IServices.CustomerPortal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Services.Business.CustomerPortal
{
    public class CustomerPortalService : ICustomerPortalService
    {
        public Task<List<ShipmentDTO>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            throw new NotImplementedException();
        }
        
        public Task<WalletTransactionSummaryDTO> GetWalletTransactions()
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceDTO> GetInvoiceByWaybill(string waybill)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InvoiceDTO>> GetInvoices()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            throw new NotImplementedException();
        }

        public Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount()
        {
            throw new NotImplementedException();
        }
    }
}
