using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.View;
using System;
using System.Linq;

namespace GIGLS.CORE.DTO.Report
{
    public class ScanTrackFilterCriteria
    {
        public string Waybill { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId { get; set; }
        public int ServiceCentreId { get; set; }

        //ScanStatus
        public string Code { get; set; }
        public string Incident { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }

        //InvoiceView
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }

        //User
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private Tuple<DateTime, DateTime> getStartDateAndEndDate()
        {
            ScanTrackFilterCriteria scanFilterCriteria = this;

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            //If No Date Supplied, Pick today date
            if (!scanFilterCriteria.StartDate.HasValue && !scanFilterCriteria.EndDate.HasValue)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has value and EndDate has Value 
            if (scanFilterCriteria.StartDate.HasValue && scanFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)scanFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                var tempEndDate = ((DateTime)scanFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            //StartDate has value and EndDate has no Value
            if (scanFilterCriteria.StartDate.HasValue && !scanFilterCriteria.EndDate.HasValue)
            {
                var tempStartDate = ((DateTime)scanFilterCriteria.StartDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day);

                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }

            //StartDate has no value and EndDate has Value
            if (scanFilterCriteria.EndDate.HasValue && !scanFilterCriteria.StartDate.HasValue)
            {
                var tempStartDate = ((DateTime)scanFilterCriteria.EndDate);
                startDate = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day).AddDays(-1); //a day ago

                var tempEndDate = ((DateTime)scanFilterCriteria.EndDate);
                endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day);
            }

            return new Tuple<DateTime, DateTime>(startDate, endDate.AddDays(1));
        }

        //Query builder
        public IQueryable<ShipmentTrackingView> GetQueryFromParameters(IQueryable<ShipmentTrackingView> queryable)
        {
            if (!string.IsNullOrWhiteSpace(Waybill))
            {
                queryable = queryable.Where(s => s.Waybill == Waybill.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Location))
            {
                queryable = queryable.Where(s => s.Location == Location.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Status))
            {
                queryable = queryable.Where(s => s.Status == Status.Trim());
            }
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                queryable = queryable.Where(s => s.UserId == UserId.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Code))
            {
                queryable = queryable.Where(s => s.Code == Code.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Incident))
            {
                queryable = queryable.Where(s => s.Incident == Incident.Trim());
            }

            if (!string.IsNullOrWhiteSpace(Reason))
            {
                queryable = queryable.Where(s => s.Reason == Reason.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Comment))
            {
                queryable = queryable.Where(s => s.Comment == Comment.Trim());
            }
            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                queryable = queryable.Where(s => s.FirstName == FirstName.Trim());
            }
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                queryable = queryable.Where(s => s.LastName == LastName.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                queryable = queryable.Where(s => s.Email == Email.Trim());
            }

            //Date Range
            if (default(DateTime) != StartDate)
            {
                queryable = queryable.Where(s => s.DateTime >= StartDate);
            }
            if (default(DateTime) != EndDate)
            {
                queryable = queryable.Where(s => s.DateTime <= EndDate);
            }

            return queryable;
        }

        public IQueryable<ShipmentTracking> GetQueryFromParameters(IQueryable<ShipmentTracking> queryable)
        {
            if (!string.IsNullOrWhiteSpace(Waybill))
            {
                queryable = queryable.Where(s => s.Waybill == Waybill.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Location))
            {
                queryable = queryable.Where(s => s.Location == Location.Trim());
            }
            if (!string.IsNullOrWhiteSpace(Status))
            {
                queryable = queryable.Where(s => s.Status == Status.Trim());
            }
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                queryable = queryable.Where(s => s.UserId == UserId.Trim());
            }
            if (ServiceCentreId > 0)
            {
                queryable = queryable.Where(s => s.ServiceCentreId == ServiceCentreId);
            }
            
            var queryDate = getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            queryable = queryable.Where(x => x.DateTime >= startDate && x.DateTime < endDate);

            return queryable;
        }
    }
}
