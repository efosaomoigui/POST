using GIGLS.Core.View.AdminReportView;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Admin
{
    public class AdminReportDTO
    {
        public List<Report_AllTimeSalesByCountry> AllTimeSalesByCountry;
        public List<Report_BusiestRoute> BusiestRoute;
        public List<Report_CustomerRevenue> CustomerRevenue;
        public List<Report_MostShippedItemByWeight> MostShippedItemByWeight;
        public List<Report_RevenuePerServiceCentre> RevenuePerServiceCentre;
        public List<Report_TotalServiceCentreByState> TotalServiceCentreByState;
    }
}
