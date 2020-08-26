using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Shipments
{
    public class ScanDTO
    {
        public string WaybillNumber { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public string CancelledOrCollected { get; set; }
    }

    public class SuperManifestScanDTO
    {
        public List<Manifest> ListOfManifests { get; set; }
        public HashSet<string> WaybillsInManifest { get; set; }
    }

}
