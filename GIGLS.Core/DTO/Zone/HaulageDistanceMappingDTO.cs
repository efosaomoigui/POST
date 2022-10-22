using POST.Core.DTO.ServiceCentres;
using POST.CORE.DTO;

namespace POST.Core.DTO.Zone
{
    public class HaulageDistanceMappingDTO : BaseDomainDTO
    {
        public int HaulageDistanceMappingId { get; set; }

        public int Distance { get; set; }

        public int DepartureId { get; set; }

        public virtual StationDTO Departure { get; set; }

        public int DestinationId { get; set; }


        public virtual StationDTO Destination { get; set; }

        public bool Status { get; set; }
    }

}
