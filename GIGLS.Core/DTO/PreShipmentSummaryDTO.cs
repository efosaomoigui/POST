using POST.Core.DTO.Partnership;
using POST.Core.DTO.Shipments;
using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class PreShipmentSummaryDTO : BaseDomainDTO
    {
        public PreShipmentMobileDTO shipmentdetails { get; set; }
        public PartnerDTO partnerdetails { get; set; }
    }
}
