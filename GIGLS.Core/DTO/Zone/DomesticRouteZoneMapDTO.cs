using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class DomesticRouteZoneMapDTO : BaseDomainDTO
    {
        public int DomesticRouteZoneMapId { get; set; }
        public int ZoneId { get; set; }
        public virtual ZoneDTO Zone { get; set; }

        public int DepartureId { get; set; }
        public StationDTO Departure { get; set; }

        public int DestinationId { get; set; }
        public StationDTO Destination { get; set; }

        public bool Status { get; set; }

        
    }
}
