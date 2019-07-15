using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.DTO.MessagingLog
{
    public class MessageFilterOption
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public MessagingLogStatus? Status { get; set; }


        /// <summary>
        /// Get the Start Date and End Date for query to the database
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public Tuple<DateTime, DateTime> getStartDateAndEndDate()
        {
            MessageFilterOption filterCriteria = this;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!filterCriteria.StartDate.HasValue && !filterCriteria.EndDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has value and EndDate has Value 
            if (filterCriteria.StartDate.HasValue && filterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)filterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                var tempEndDate = ((DateTime)filterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            //StartDate has value and EndDate has no Value
            if (filterCriteria.StartDate.HasValue && !filterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)filterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has no value and EndDate has Value
            if (filterCriteria.EndDate.HasValue && !filterCriteria.StartDate.HasValue)
            {
                startDate = new DateTime(2000, DateTime.Now.Month, DateTime.Now.Day);

                var tempEndDate = ((DateTime)filterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            return new Tuple<DateTime, DateTime>(startDate, endDate.AddDays(1));
        }
    }
}
