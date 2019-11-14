using System;

namespace GIGLS.CORE.DTO.Report
{
    public class BaseFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ServiceCentreId { get; set; }
        public int StationId { get; set; }
        public int StateId { get; set; }
        public bool fromGigGoDashboard { get; set; }

        /// <summary>
        /// Get the Start Date and End Date for query to the database
        /// </summary>
        /// <param name="accountFilterCriteria"></param>
        /// <returns></returns>
        public Tuple<DateTime, DateTime> getStartDateAndEndDate()
        {
            BaseFilterCriteria accountFilterCriteria = this;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                startDate = new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has value and EndDate has Value 
            if (accountFilterCriteria.StartDate.HasValue && accountFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)accountFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                var tempEndDate = ((DateTime)accountFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            //StartDate has value and EndDate has no Value
            if (accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)accountFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has no value and EndDate has Value
            if (accountFilterCriteria.EndDate.HasValue && !accountFilterCriteria.StartDate.HasValue)
            {
                startDate = new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day);

                var tempEndDate = ((DateTime)accountFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            return new Tuple<DateTime, DateTime>(startDate, endDate.AddDays(1));
        }

        /// <summary>
        /// Get the Start Date and End Date for query to the database
        /// </summary>
        /// <param name="accountFilterCriteria"></param> 
        /// <returns></returns>
        public Tuple<DateTime, DateTime> getStartDateAndEndDate2(DateTime dt) 
        {

            BaseFilterCriteria accountFilterCriteria = this;

            var today = DateTime.Now;
            var startDate = dt;
            var endDate = today; //startDate.AddDays(limit);
            

            //If No Date Supplied
            //if (!accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            //{
            //    startDate = DateTime.Now.AddDays(-1 * limit);
            //    endDate = today; //startDate.AddDays(limit);
            //}

            //StartDate has value and EndDate has Value 
            //if (accountFilterCriteria.StartDate.HasValue && accountFilterCriteria.EndDate.HasValue)
            //{
            //    var tempStartDate = ((DateTime)accountFilterCriteria.StartDate);
            //    startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

            //    var tempEndDate = ((DateTime)accountFilterCriteria.EndDate);
            //    endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            //}

            //StartDate has value and EndDate has no Value
            //if (accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            //{
            //    var tempStartDate = ((DateTime)accountFilterCriteria.StartDate);
            //    startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

            //    endDate = startDate.AddDays(limit);
            //}

            //StartDate has no value and EndDate has Value
            //if (accountFilterCriteria.EndDate.HasValue && !accountFilterCriteria.StartDate.HasValue)
            //{
            //    startDate = new DateTime(2019, DateTime.Now.Month, 1);

            //    var tempEndDate = ((DateTime)accountFilterCriteria.EndDate);
            //    endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            //}

            return new Tuple<DateTime, DateTime>(startDate, endDate);
        }
    }
}
