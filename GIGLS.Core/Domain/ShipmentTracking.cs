using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class ShipmentTracking : BaseDomain
    {
        public int ShipmentTrackingId { get; set; }
        public string Waybill { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public TrackingType TrackingType  { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}