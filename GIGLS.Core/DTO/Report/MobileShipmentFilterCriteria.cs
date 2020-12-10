using System;

namespace GIGLS.Core.DTO.Report
{
    public class MobileShipmentFilterCriteria
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }
        public int CountryId { get; set; }
        public string VehicleType { get; set; }
        public string CompanyType { get; set; }
        public string Shipmentstatus { get; set; }

        /// <summary>
        /// Get the Start Date and End Date for query to the database
        /// </summary>
        public Tuple<DateTime, DateTime> getStartDateAndEndDate()
        {
            MobileShipmentFilterCriteria accountFilterCriteria = this;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied
            if (!accountFilterCriteria.StartDate.HasValue && !accountFilterCriteria.EndDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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
    }
}