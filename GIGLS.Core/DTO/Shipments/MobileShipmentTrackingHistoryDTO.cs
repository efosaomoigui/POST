using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class MobileShipmentTrackingHistoryDTO
    {
        public string Origin { get; set; }
        public string Destination { get; set; }

        public List<MobileShipmentTrackingDTO> MobileShipmentTrackings { get; set; }
    }
}
