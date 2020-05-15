using System;

namespace GIGLS.CORE.DTO.Report
{
    public class DateFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CountryId { get; set; }

        /// <summary>
        /// Get the Start Date and End Date for query to the database
        /// </summary>
        /// <param name="dateFilterCriteria"></param>
        /// <returns></returns>
        public Tuple<DateTime, DateTime> getStartDateAndEndDate()
        {
            DateFilterCriteria dateFilterCriteria = this;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!dateFilterCriteria.StartDate.HasValue && !dateFilterCriteria.EndDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has value and EndDate has Value 
            if (dateFilterCriteria.StartDate.HasValue && dateFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)dateFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                var tempEndDate = ((DateTime)dateFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            //StartDate has value and EndDate has no Value
            if (dateFilterCriteria.StartDate.HasValue && !dateFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)dateFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has no value and EndDate has Value
            if (dateFilterCriteria.EndDate.HasValue && !dateFilterCriteria.StartDate.HasValue)
            {
                startDate = new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day);

                var tempEndDate = ((DateTime)dateFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            return new Tuple<DateTime, DateTime>(startDate, endDate.AddDays(1));
        }
    }
}
