using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;

namespace GIGLS.Core.Domain
{
    public class Region : BaseDomain, IAuditable
    {
        public Region()
        {
            ServiceCentres = new HashSet<ServiceCentre>();
        }

        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public virtual ICollection<ServiceCentre> ServiceCentres { get; set; }
    }
}
