using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class ManifestGroupWaybillNumberMappingDTO : BaseDomainDTO
    {
        public int ManifestGroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string ManifestCode { get; set; }
        public string GroupWaybillNumber { get; set; }
        public List<string> GroupWaybillNumbers { get; set; }
    }
}
