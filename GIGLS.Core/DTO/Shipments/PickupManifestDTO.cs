using POST.Core.DTO.Fleets;
using POST.Core.DTO.ServiceCentres;
using POST.Core.Enums;
using POST.CORE.DTO;
using System;
using System.Collections.Generic;

namespace POST.Core.DTO.Shipments
{
    public class PickupManifestDTO : BaseDomainDTO
    {
        public PickupManifestDTO()
        {
            FleetTrip = new List<FleetTripDTO>();
            PreShipments = new List<PreShipmentMobileDTO>();
        }
        public int PickupManifestId { get; set; }
        public string ManifestCode { get; set; }
        public DateTime DateTime { get; set; }
        public string MasterWaybill { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceiverBy { get; set; }
        public int? ShipmentId { get; set; }
        public int? FleetTripId { get; set; }
        public List<FleetTripDTO> FleetTrip { get; set; }
        public List<PreShipmentMobileDTO> PreShipments { get; set; }
        public bool IsDispatched { get; set; }
        public bool IsReceived { get; set; }
        public ManifestType ManifestType { get; set; }
        public ManifestStatus ManifestStatus { get; set; }

        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }
    }
}
