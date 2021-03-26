using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class CountryRouteZoneMap : BaseDomain, IAuditable
    {
        public int CountryRouteZoneMapId { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        
        public int DepartureId { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Country Departure { get; set; }

        public int DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public virtual Country Destination { get; set; }

        public double Rate { get; set; }

        public bool Status { get; set; }
        public CompanyMap CompanyMap { get; set; }
    }
}
