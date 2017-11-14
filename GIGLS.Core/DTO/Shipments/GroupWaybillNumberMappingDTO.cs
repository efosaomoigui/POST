using GIGLS.Core.Domain;
using System;

namespace GIGLS.CORE.DTO.Shipments
{
    public class GroupWaybillNumberMappingDTO : BaseDomain
    {
        public int GroupWaybillNumberMappingId { get; set; }
        public DateTime DateMapped { get; set; }
        public bool IsActive { get; set; }

        public string GroupWaybillNumber { get; set; }
        public string WaybillNumber { get; set; }
    }
}