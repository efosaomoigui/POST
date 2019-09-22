using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.View.AdminReportView
{
    public class Report_AllTimeSalesByCountry
    {
        public decimal GrandTotal { get; set; }
        public int TotalOrders { get; set; }

        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CurrencySymbol { get; set; }
    }

    public class Report_BusiestRoute
    {
        [Key]
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public int TotalShipment { get; set; }
    }

    public class Report_CustomerRevenue
    {
        [Key]
        public string CompanyType { get; set; }
        public int Total { get; set; }
        public decimal Revenue { get; set; }
    }

    public class Report_MostShippedItemByWeight
    {
        [Key]
        public double Weight { get; set; }
        public int Total { get; set; }
    }

    public class Report_RevenuePerServiceCentre
    {
        [Key]
        public int ServiceCentreId { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
    }

    public class Report_TotalServiceCentreByState
    {        
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int TotalService { get; set; }
    }

    public class Report_TotalOrdersDelivered
    {
        [Key]
        public int TotalOrdered { get; set; }
    }
}