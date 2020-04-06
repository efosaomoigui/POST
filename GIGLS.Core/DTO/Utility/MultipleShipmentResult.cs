using System.Collections.Generic;

namespace GIGLS.Core.DTO.Utility
{
    public class MultipleShipmentResult
    {
        public string Waybill { get; set; }
        public int ZoneMapping { get; set; }
    }

    public class MultipleShipmentOutput
    {
        public MultipleShipmentOutput()
        {
            Waybills = new HashSet<MultipleShipmentResult>();
        }
        public string groupCodeNumber { get; set; }
        public HashSet<MultipleShipmentResult> Waybills { get; set; }
    }
}
