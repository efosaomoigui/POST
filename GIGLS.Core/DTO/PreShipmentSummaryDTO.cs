using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class PreShipmentSummaryDTO : BaseDomainDTO
    {
        public PreShipmentMobileDTO shipmentdetails { get; set; }
        public PartnerDTO partnerdetails { get; set; }

        public string PictureUrl { get; set; }
    }
}
