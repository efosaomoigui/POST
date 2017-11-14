using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain
{
    public class FleetMovement : BaseDomain, IAuditable
    {
        public int FleetMovementId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Station Departure { get; set; }
        public int DepartureId { get; set; }

        [ForeignKey("DestinationId")]
        public virtual Station Destination { get; set; }
        public int DestinationId { get; set; }
        
        //who assign fleet to this route
        //public int UserId { get; set; }
        //public virtual User User { get; set; }
    }
}
