using GIGLS.Core.DTO.Fleets;
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
        public int DispatchedBy { get; set; }
        public int ReceiverBy { get; set; }
        public int ShipmentId { get; set; }
        public int FleetTripId { get; set; }
        public List<FleetTripDTO> FleetTrip { get; set; }
        public List<ShipmentDTO> Shipments { get; set; }
    }
}
