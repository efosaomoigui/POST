using System;

namespace GIGLS.Core.Domain
{
    public class ManifestGroupWaybillNumberMapping : BaseDomain
    {
        public int ManifestGroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public string GroupWaybillNumber { get; set; }
    }
}
