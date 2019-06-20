using GIGL.GIGLS.Core.Domain;

namespace GIGLS.Core.Domain
{
    public class HUBManifestWaybillMapping : BaseDomain
    {
        public int HUBManifestWaybillMappingId { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public string Waybill { get; set; }

        //This will be used for better filtering of data 
        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }
    }
}
