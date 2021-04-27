using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.View;
using System.Collections.Generic;
using System.Linq;

namespace GIGLS.Core.DTO.Dashboard
{
    public class DashboardDTO
    {
        public int TotalShipmentDelivered { get; set; }
        public int TotalShipmentOrdered { get; set; }
        public int TotalShipmentAwaitingCollection { get; set; }
        public int TotalShipmentExpected { get; set; }
        public int TotalCustomers { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }
        public StationDTO Station { get; set; }
        public RegionDTO Region { get; set; }
        public string Public { get; set; }
        public List<ShipmentOrderDTO> MostRecentOrder { get; set; }
        public List<GraphDataDTO> GraphData { get; set; }
        public GraphDataDTO CurrentMonthGraphData { get; set; }
        public IQueryable<InvoiceView> ShipmentsOrderedByServiceCenter { get; set; }
        public IQueryable<MagayaShipmentAgility> MagayaShipmentOrdered { get; set; }  
        public decimal TargetAmount { get; set; }
        public int TargetOrder { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal OutstandingCorporatePayment { get; set; }
        public EarningsBreakdownDTO EarningsBreakdownDTO { get; set; }

        public CountryDTO UserActiveCountry { get; set; }
        public List<CountryDTO> ActiveCountries { get; set; }
        public CountryDTO UserActiveCountryForFilter { get; set; }
        public WalletTransactionSummary WalletTransactionSummary { get; set; }
        public WalletPaymentLogSummary WalletPaymentLogSummary { get; set; }
        public WalletBreakdown WalletBreakdown { get; set; }
        public bool DashboardAccess { get; set; }  
        public CustomerBreakdownDTO CustomerBreakdownDTO { get; set; }
        public ServiceCentreBreakdownDTO ServiceCentreBreakdownDTO { get; set; }
        public int TotalMonthlyShipmentOrdered { get; set; }
        public int TotalDailyShipmentOrdered { get; set; }
        public double TotalMonthlyWeightOfShipmentOrdered { get; set; }
        public double TotalDailyWeightOfShipmentOrdered { get; set; }
        public EarningsBreakdownByCustomerDTO EarningsBreakdownByCustomerDTO { get; set; }
        public EarningsBreakdownOfEcommerceDTO EarningsBreakdownOfEcommerceDTO { get; set; }
        public int ClassSubscriptionsCount { get; set; }
        public OutboundShipmentsReportDTO OutboundShipmentsReportDTO   { get; set; }
        public InboundShipmentsReportDTO InboundShipmentsReportDTO { get; set; }
    }

    public class CustomerBreakdownDTO
    {
        public int Individual { get; set; }
        public int EcommerceBasic { get; set; }
        public int EcommerceClass { get; set; }
        public int Corporate { get; set; }
    }

    public class ServiceCentreBreakdownDTO
    {
        public int WalkIn { get; set; }
        public int Hub { get; set; }
        public int Gateway { get; set; }
        public int Total { get; set; }
    }

}

