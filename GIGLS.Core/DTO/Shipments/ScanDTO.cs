using GIGL.POST.Core.Domain;
using POST.Core.Enums;
using System.Collections.Generic;

namespace POST.Core.DTO.Shipments
{
    public class ScanDTO
    {
        public string WaybillNumber { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public string CancelledOrCollected { get; set; }
    }

    public class ScanDTO2
    {
        public string[] WaybillNumber { get; set; }
        public ShipmentScanStatus ShipmentScanStatus { get; set; }
        public string CancelledOrCollected { get; set; }
    }

    public class SuperManifestScanDTO
    {
        public List<Manifest> ListOfManifests { get; set; }
        public HashSet<string> WaybillsInManifest { get; set; }
    }

}
