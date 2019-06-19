using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class ManifestDTO : BaseDomainDTO
    {
        public ManifestDTO()
        {
            FleetTrip = new List<FleetTripDTO>();
            Shipments = new List<ShipmentDTO>();
        }
        public int ManifestId { get; set; }
        public string ManifestCode { get; set; }
        public DateTime DateTime { get; set; }
        public string MasterWaybill { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceiverBy { get; set; }
        public int? ShipmentId { get; set; }
        public int? FleetTripId { get; set; }
        public List<FleetTripDTO> FleetTrip { get; set; }
        public List<ShipmentDTO> Shipments { get; set; }
        public bool IsDispatched { get; set; }
        public bool IsReceived { get; set; }
        public ManifestType ManifestType { get; set; }

        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
    }
}
