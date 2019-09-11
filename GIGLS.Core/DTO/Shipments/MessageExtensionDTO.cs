using GIGLS.Core.Enums;

namespace GIGLS.Core.DTO.Shipments
{
    public class MessageExtensionDTO
    {
        public string RegionalManagerName { get; set; }
        public string RegionalManagerEmail { get; set; }
        public string ServiceCenterAgentName { get; set; }
        public string ServiceCenterName { get; set; }
        public string ScanStatus { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public string WaybillNumber { get; set; }
        public string CancelledOrCollected { get; set; }
        public string Manifest { get; set; }
        public string GroupWaybill { get; set; }
    }
}
