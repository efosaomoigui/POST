using POST.CORE.DTO;
using System;

namespace POST.Core.DTO.Shipments
{
    public class ShipmentCancelDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ShipmentCreatedDate { get; set; }
        public string CancelledBy { get; set; }
        public string CancelReason { get; set; }
    }
}
