using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.ShipmentScan
{
   public class MobileScanStatusDTO : BaseDomainDTO
    {
        public int MobileScanStatusId { get; set; }
        public string Code { get; set; }
        public string Incident { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
    }
}
