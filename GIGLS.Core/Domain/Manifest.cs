using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class Manifest : BaseDomain
    {
        public int ManifestId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string ManifestCode { get; set; }
        public DateTime DateTime { get; set; }

        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public int DispatchedById { get; set; }
        public virtual User DispatchedBy { get; set; }

        public int ReceiverById { get; set; }
        public virtual User ReceiverBy { get; set; }

        public int FleetTripId { get; set; }
        public virtual FleetTrip FleetTrip { get; set; }
    }
}
