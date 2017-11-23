using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentTrackingDTO : BaseDomainDTO
    {
        public int ShipmentTrackingId { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public string TrackingType { get; set; }
        public string User { get; set; }
        public string Waybill { get; set; }
    }
}
