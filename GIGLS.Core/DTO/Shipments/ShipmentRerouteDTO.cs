using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class ShipmentRerouteDTO : BaseDomainDTO
    {
        public string WaybillNew { get; set; }        
        public string WaybillOld { get; set; }
        public string RerouteBy { get; set; }
        public ShipmentRerouteInitiator ShipmentRerouteInitiator { get; set; }
    }
}
