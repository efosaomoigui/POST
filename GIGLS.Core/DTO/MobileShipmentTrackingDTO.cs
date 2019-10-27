using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class MobileShipmentTrackingDTO
    {
        public int MobileShipmentTrackingId { get; set; }
        public string Waybill { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public TrackingType TrackingType { get; set; }
        public MobileScanStatusDTO ScanStatus { get; set; }

        public string User { get; set; }
        public int ServiceCentreId { get; set; }
    }
}
