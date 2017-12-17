using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Fleets
{
    public class DispatchDTO : BaseDomainDTO
    {
        public int DispatchId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ManifestNumber { get; set; }
        public decimal Amount { get; set; }
        public int RescuedDispatchId { get; set; }
        public DispatchDTO RescuedDispatch { get; set; }
        public string DriverDetail { get; set; }
        public string DispatchedBy { get; set; }
        public string ReceivedBy { get; set; }
        public DispatchCategory DispatchCategory { get; set; }

        public int? DepartureId { get; set; }
        public StationDTO Departure { get; set; }
        
        public int? DestinationId { get; set; }
        public StationDTO Destination { get; set; }
    }
}
