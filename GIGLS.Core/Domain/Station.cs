using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;
using System;

namespace GIGLS.Core.Domain
{
    public class Station : BaseDomain, IAuditable
    {
        public Station()
        {
            ServiceCentres = new HashSet<ServiceCentre>();
        }

        public int StationId { get; set; }

        public string StationName { get; set; }
        public string StationCode { get; set; }
        
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public virtual ICollection<ServiceCentre> ServiceCentres { get; set; }
    }
}
