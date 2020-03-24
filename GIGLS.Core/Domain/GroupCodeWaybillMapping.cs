using GIGLS.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class GroupCodeWaybillMapping : BaseDomain
    {
        public int GroupCodeWaybillMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(100), MinLength(5)]
        public string GroupCodeNumber { get; set; }

        [MaxLength(100), MinLength(5)]
        public string WaybillNumber { get; set; }
    }
}