using GIGLS.Core;
using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class HaulageDistanceMapping : BaseDomain, IAuditable
    {
        public int HaulageDistanceMappingId { get; set; }

        public int Distance { get; set; }

        public int? DepartureId { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Station Departure { get; set; }

        public int? DestinationId { get; set; }


        [ForeignKey("DestinationId")]
        public virtual Station Destination { get; set; }

        public bool Status { get; set; }
    }

}