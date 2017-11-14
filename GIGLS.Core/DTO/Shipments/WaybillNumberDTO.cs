using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Shipments
{
    public class WaybillNumberDTO : BaseDomainDTO
    {
        public int WaybillNumberId { get; set; }
        public string WaybillCode { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual string ServiceCentre { get; set; }
    }
}
