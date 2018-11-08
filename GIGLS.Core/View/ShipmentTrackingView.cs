using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.View
{
    public class ShipmentTrackingView : BaseDomainDTO
    {
        [Key]
        public int ShipmentTrackingId { get; set; }
        public string Waybill { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public TrackingType TrackingType { get; set; }
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
    }
}
