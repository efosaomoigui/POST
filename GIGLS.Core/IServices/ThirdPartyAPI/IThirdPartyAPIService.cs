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
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ThirdPartyAPI
{
    public interface IThirdPartyAPIService : IServiceDependencyMarker
    {
        //Price API
        Task<decimal> GetPrice2(ThirdPartyPricingDTO thirdPartyPricingDto);
        Task<decimal> GetHaulagePrice(HaulagePricingDTO pricingDto);

        //Capture PreShipment API
        Task<PreShipmentDTO> AddPreShipment(ThirdPartyPreShipmentDTO thirdPartyPreShipmentDTO);

        //Route API


        //Track API
        Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber);
        Task<IEnumerable<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber);

        //Invoice API
        Task<IEnumerable<InvoiceViewDTO>> GetInvoices();
        Task<InvoiceDTO> GetInvoiceByWaybill(string waybill);

        //Transaction History API
        Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria);
        Task<WalletTransactionSummaryDTO> GetWalletTransactions();
        Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount();
        Task<IEnumerable<PaymentPartialTransactionDTO>> GetPartialPaymentTransaction(string waybill);

        //General API
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

        Task<CustomerDTO> GetCustomer(string userId);
        Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword);
        Task<IEnumerable<StationDTO>> GetLocalStations();
        Task<IEnumerable<StationDTO>> GetInternationalStations();

        Task<PreShipmentMobileDTO> GetPrice(PreShipmentMobileDTO preShipment);
        Task<PreShipmentMobileDTO> CreatePreShipment(PreShipmentMobileDTO preShipmentDTO);

    }
}
