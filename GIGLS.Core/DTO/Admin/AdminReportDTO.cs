using POST.Core.DTO.Account;
using POST.Core.View.AdminReportView;
using System.Collections.Generic;

namespace POST.Core.DTO.Admin
{
    public class AdminReportDTO
    {
        public AdminReportDTO()
        {
            NumberOfCustomer = new CustomersCount();
        }
        //public List<Report_AllTimeSalesByCountry> AllTimeSalesByCountry;
        public List<Report_BusiestRoute> BusiestRoute;
        //public List<Report_CustomerRevenue> CustomerRevenue;
        public List<Report_MostShippedItemByWeight> MostShippedItemByWeight;
        //public List<Report_RevenuePerServiceCentre> RevenuePerServiceCentre;
        public List<Report_TotalServiceCentreByState> TotalServiceCentreByState;
        public Report_TotalOrdersDelivered TotalOrdersDelivered;
        public CustomersCount NumberOfCustomer;
        public int TotalCustomers;
        public InvoiceReportDTO InvoiceReportDTO;
    }

    public class CustomersCount
    {
        public int Ecommerce { get; set; }
        public int Corporate { get; set; }
        public int Individual { get; set; }
    }
}
