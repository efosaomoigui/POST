using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class Location : BaseDomain, IAuditable
    {
        public int LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string FormattedAddress { get; set; }
        [MaxLength(128)]
        public string LGA { get; set; }

    }
}
