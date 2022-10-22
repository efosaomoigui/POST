using POST.Core.Enums;
using POST.CORE.DTO;

namespace POST.Core.DTO.Shipments
{
    public class ShipmentRerouteDTO : BaseDomainDTO
    {
        public string WaybillNew { get; set; }        
        public string WaybillOld { get; set; }
        public string RerouteBy { get; set; }
        public string RerouteReason { get; set; }
        public ShipmentRerouteInitiator ShipmentRerouteInitiator { get; set; }
    }
}