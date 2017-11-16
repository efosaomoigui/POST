using GIGLS.Core.Enums;

namespace GIGLS.Core.DTO.Shipments
{
    public class ScanDTO
    {
        public string WaybillNumber { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
    }
}
