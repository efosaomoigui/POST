using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class ShipmentTracking : BaseDomain
    {
        public int ShipmentTrackingId { get; set; }

        [MaxLength(100)]
        [Index]
        public string Waybill { get; set; }

        public string Location { get; set; }

        [MaxLength(100)]
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public TrackingType TrackingType  { get; set; }
        
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int ServiceCentreId { get; set; }
    }
}