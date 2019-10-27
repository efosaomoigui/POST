using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class ManifestGroupWaybillNumberMapping : BaseDomain
    {
        public int ManifestGroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100), MinLength(5)]
        public string ManifestCode { get; set; }

        [MaxLength(100), MinLength(5)]
        public string GroupWaybillNumber { get; set; }
    }
}
