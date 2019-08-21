using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class PickupManifest : BaseDomain
    {
        public int ManifestId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string ManifestCode { get; set; }
        public DateTime DateTime { get; set; }

        public int? ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public string DispatchedById { get; set; }

        public string ReceiverById { get; set; }

        public int? FleetTripId { get; set; }
        public virtual FleetTrip FleetTrip { get; set; }

        public bool IsDispatched { get; set; }
        public bool IsReceived { get; set; }
        public int ServiceCentreId { get; set; }
        public ManifestType ManifestType { get; set; }

        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
    }
}
