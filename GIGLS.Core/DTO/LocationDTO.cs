using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO
{
    public class LocationDTO : BaseDomainDTO
    {
        public int LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public double OriginLatitude { get; set; }
        public double OriginLongitude { get; set; }

        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
    }
}
