using GIGLS.Core.DTO.ShipmentScan;
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
        public ScanStatusDTO ScanStatus { get; set; }

        public string Destination { get; set; }
        public string Departure { get; set; }
        public string DeliveryOptions { get; set; }
        public string PickupOptions { get; set; }
        public decimal Amount { get; set; }        
    }
}
