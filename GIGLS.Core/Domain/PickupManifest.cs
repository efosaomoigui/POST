using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class PickupManifest : BaseDomain
    {
        public int PickupManifestId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string ManifestCode { get; set; }
        public DateTime DateTime { get; set; }

        public int? ShipmentId { get; set; }
        public PreShipmentMobile Shipment { get; set; }

        [MaxLength(128)]
        public string DispatchedById { get; set; }

        [MaxLength(128)]
        public string ReceiverById { get; set; }

        public int? FleetTripId { get; set; }
        public virtual FleetTrip FleetTrip { get; set; }

        public bool IsDispatched { get; set; }
        public bool IsReceived { get; set; }
        public int ServiceCentreId { get; set; }
        public ManifestType ManifestType { get; set; }
        public ManifestStatus ManifestStatus { get; set; }

        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public bool Picked { get; set; }
    }
}
