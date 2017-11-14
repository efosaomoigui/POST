using GIGLS.Core.Domain;
using System;

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
        
        public int CaptainId { get; set; }
        public virtual User Captain { get; set; }
    }
}