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
    }
}
