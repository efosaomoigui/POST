using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.Enums;
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
        public string Manifest { get; set; }
        public string ManifestType { get; set; }
        public ScanStatusDTO ScanStatus { get; set; }

        //
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }

        public decimal Amount { get; set; }
        public string PickupOptions { get; set; }
        public string DeliveryOptions { get; set; }

        public int ServiceCentreId { get; set; }
    }
}
