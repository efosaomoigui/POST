using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using Microsoft.AspNet.Identity;
using System;
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
        Task<IEnumerable<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber);
        Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount();
        Task<IEnumerable<PaymentPartialTransactionDTO>> GetPartialPaymentTransaction(string waybill);
        Task<DashboardDTO> GetDashboard();

        //For Quick Quotes
        Task<IEnumerable<StateDTO>> GetStates(int pageSize = 10, int page = 1);
        int GetStatesTotal();
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres();
        Task<IEnumerable<DeliveryOptionDTO>> GetDeliveryOptions();
        Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages();
        Task<IEnumerable<HaulageDTO>> GetHaulages();
        Task<IEnumerable<InsuranceDTO>> GetInsurances();
        Task<IEnumerable<VATDTO>> GetVATs();
        Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination);
        Task<decimal> GetPrice(PricingDTO pricingDto);
        Task<decimal> GetHaulagePrice(HaulagePricingDTO pricingDto);
        Task<CustomerDTO> GetCustomer(string userId);
        Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword);
        Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO);

        Task<List<PreShipmentDTO>> GetPreShipments(FilterOptionsDto filterOptionsDto);
        Task<PreShipmentDTO> GetPreShipment(string waybill);
    }
}
