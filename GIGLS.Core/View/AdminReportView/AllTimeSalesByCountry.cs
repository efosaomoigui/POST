namespace GIGLS.Core.View.AdminReportView
{
    public class Report_AllTimeSalesByCountry
    {
        public double GrandTotal { get; set; }
        public int TotalOrders { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CurrencySymbol { get; set; }
    }


    public class Report_BusiestRoute
    {
        public string DepartureStation { get; set; }
        public string DestinationStation { get; set; }
        public double TotalShipment { get; set; }
    }

    public class Report_CustomerRevenue
    {
        public string CompanyType { get; set; }
        public int Total { get; set; }
        public double Revenue { get; set; }
    }

    public class Report_MostShippedItemByWeight
    {
        public double Weight { get; set; }
        public int Total { get; set; }
    }

    public class Report_RevenuePerServiceCentre
    {
        public int ServiceCentreId { get; set; }
        public int Name { get; set; }
        public double Total { get; set; }
    }

    public class Report_TotalServiceCentreByState
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public double TotalService { get; set; }
    }
}