using GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class GroupWaybillNumberMapping: BaseDomain
    {
        public int GroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string GroupWaybillNumber { get; set; }
        public string WaybillNumber { get; set; }
    }
}