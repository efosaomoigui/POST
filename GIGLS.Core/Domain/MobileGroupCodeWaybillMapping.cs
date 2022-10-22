using POST.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGL.POST.Core.Domain
{
    public class MobileGroupCodeWaybillMapping : BaseDomain
    {
        public int MobileGroupCodeWaybillMappingId { get; set; }
        public DateTime DateMapped { get; set; }

        [MaxLength(100), MinLength(5)]
        public string GroupCodeNumber { get; set; }

        [MaxLength(100), MinLength(5)]
        public string WaybillNumber { get; set; }
    }
}