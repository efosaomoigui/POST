using System;

namespace GIGLS.Core.DTO.Report
{
    public class DashboardFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Get the Start Date and End Date for query to the database
        /// </summary>
        /// <param name="dashboardFilterCriteria"></param>
        /// <returns></returns>
        public Tuple<DateTime, DateTime> getStartDateAndEndDate()
        {
            DashboardFilterCriteria dashboardFilterCriteria = this;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!dashboardFilterCriteria.StartDate.HasValue && !dashboardFilterCriteria.EndDate.HasValue)
            {
                var threeMonthAgo = DateTime.Now.AddMonths(-3);  //Three (3) Months ago
                startDate = new DateTime(threeMonthAgo.Year, threeMonthAgo.Month, 1);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has value and EndDate has Value 
            if (dashboardFilterCriteria.StartDate.HasValue && dashboardFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)dashboardFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                var tempEndDate = ((DateTime)dashboardFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            //StartDate has value and EndDate has no Value
            if (dashboardFilterCriteria.StartDate.HasValue && !dashboardFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)dashboardFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);
                
               // endDate = new DateTime(tempStartDate.Year, 12, 1); //last month of the year selected
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has no value and EndDate has Value
            if (dashboardFilterCriteria.EndDate.HasValue && !dashboardFilterCriteria.StartDate.HasValue)
            {
                var tempStartDate = ((DateTime)dashboardFilterCriteria.EndDate);

                var threeMonthAgoOfTempEndDate = tempStartDate.AddMonths(-3); //Three (3) Months ago
                startDate = new DateTime(threeMonthAgoOfTempEndDate.Year, threeMonthAgoOfTempEndDate.Month, 1);

                var tempEndDate = ((DateTime)dashboardFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            return new Tuple<DateTime, DateTime>(startDate, endDate.AddDays(1));
        }

        public int? ActiveCountryId { get; set; }
    }
}
