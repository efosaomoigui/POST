using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class Station : BaseDomain, IAuditable
    {
        public Station()
        {
            ServiceCentres = new HashSet<ServiceCentre>();
        }

        public int StationId { get; set; }

        [MaxLength(100)]
        public string StationName { get; set; }

        [MaxLength(100)]
        public string StationCode { get; set; }
        
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public int SuperServiceCentreId { get; set; }

        public virtual ICollection<ServiceCentre> ServiceCentres { get; set; }

        public decimal StationPickupPrice { get; set; }

        public bool GIGGoActive { get; set; }
    }
}
