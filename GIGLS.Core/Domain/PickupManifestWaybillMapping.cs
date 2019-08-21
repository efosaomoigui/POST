using GIGL.GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class PickupManifestWaybillMapping : BaseDomain
    {
        public int PickupManifestWaybillMappingId { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100), MinLength(5)]
        public string ManifestCode { get; set; }

        [MaxLength(100), MinLength(5)]
        public string Waybill { get; set; }

        //This will be used for better filtering of data 
        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }
    }
}
