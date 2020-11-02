using GIGL.GIGLS.Core.Domain;
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

        public CountryDTO UserActiveCountry { get; set; }
        public List<CountryDTO> ActiveCountries { get; set; }
        public CountryDTO UserActiveCountryForFilter { get; set; }
        public WalletTransactionSummary WalletTransactionSummary { get; set; }
    }

}

