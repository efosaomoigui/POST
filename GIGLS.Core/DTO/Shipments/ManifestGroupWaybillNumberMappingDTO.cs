using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Shipments
{
    public class ManifestGroupWaybillNumberMappingDTO : BaseDomainDTO
    {
        public int ManifestGroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public string GroupWaybillNumber { get; set; }
    }
}
