using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class HUBManifestWaybillMapping : BaseDomain
    {
        public int HUBManifestWaybillMappingId { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100)]
        public string ManifestCode { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }

        //This will be used for better filtering of data 
        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }
    }
}
