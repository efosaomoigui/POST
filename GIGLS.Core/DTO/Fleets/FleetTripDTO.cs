using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetTripDTO : BaseDomainDTO
    {
        public FleetTripDTO()
        {
            FleetDetail = new List<FleetDTO>();
        }
        public int FleetTripId { get; set; }
        public string DepartureDestination { get; set; }
        public string ActualDestination { get; set; }
        public string ExpectedDestination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal DistanceTravelled { get; set; }
        public decimal FuelCosts { get; set; }
        public decimal FuelUsed { get; set; }
        public string FleetRegistrationNumber { get; set; }
        public string CaptainName { get; set; }
        public int CaptainId { get; set; }
        public int FleetId { get; set; }
        public List<FleetDTO> FleetDetail { get; set; }
    }
}
