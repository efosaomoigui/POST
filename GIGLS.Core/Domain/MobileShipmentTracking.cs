using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class MobileShipmentTracking : BaseDomain
    {
        public int MobileShipmentTrackingId { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public string Location { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public TrackingType TrackingType { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ServiceCentreId { get; set; }
    }
}
