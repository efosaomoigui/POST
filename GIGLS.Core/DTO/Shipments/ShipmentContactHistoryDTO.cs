using POST.Core.Enums;
using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Shipments
{
    public class ShipmentContactHistoryDTO : BaseDomainDTO
    {
        public int ShipmentContactHistoryId { get; set; }
        public string Waybill { get; set; }
        public string ContactedBy { get; set; }
        public int NoOfContact { get; set; }
        public string UserId { get; set; }
    }
}
