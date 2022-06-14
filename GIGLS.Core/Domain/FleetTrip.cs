using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class FleetTrip: BaseDomain
    {
        public int FleetTripId { get; set; }
        public string DepartureDestination { get; set; }
        public string ActualDestination { get; set; }
        public string ExpectedDestination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal DistanceTravelled { get; set; }
        public decimal FuelCosts { get; set; }
        public decimal FuelUsed { get; set; }

        public int FleetId { get; set; }
        public virtual Fleet Fleet { get; set; }
        
        public string CaptainId { get; set; }

        [ForeignKey("CaptainId")]
        public virtual User Captain { get; set; }

        public int MovementManifestId { get; set; }
        public string FleetRegistrationNumber { get; set; }

        public decimal DispatchAmount { get; set; }

        public int? DepartureStationId { get; set; }

        [ForeignKey("DepartureStationId")]
        public virtual Station DepartureStation { get; set; }

        public int? DestinationStationId { get; set; }

        [ForeignKey("DestinationStationId")]
        public virtual Station DestinationStation { get; set; }
        public int DepartureServiceCenterId { get; set; }
        public int DestinationServiceCenterId { get; set; }
        public decimal TripAmount { get; set; }
    }
}