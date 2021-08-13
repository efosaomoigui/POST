using GIGLS.Core;
using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class DomesticRouteZoneMap : BaseDomain, IAuditable
    {
        public int DomesticRouteZoneMapId { get; set; }

        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; }

        
        public int DepartureId { get; set; }

        [ForeignKey("DepartureId")]
        public virtual Station Departure { get; set; }

        public int DestinationId { get; set; }


        [ForeignKey("DestinationId")]
        public virtual Station Destination { get; set; }

        public bool Status { get; set; }

        //Estimated Time of Arrival
        public int ETA { get; set; }


    }
}