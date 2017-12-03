using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class HaulageDistanceMappingDTO : BaseDomainDTO
    {
        public int HaulageDistanceMappingId { get; set; }

        public int ZoneId { get; set; }
        public virtual ZoneDTO Zone { get; set; }


        public int DepartureId { get; set; }

        public virtual StationDTO Departure { get; set; }

        public int DestinationId { get; set; }


        public virtual StationDTO Destination { get; set; }

        public bool Status { get; set; }
    }

}
