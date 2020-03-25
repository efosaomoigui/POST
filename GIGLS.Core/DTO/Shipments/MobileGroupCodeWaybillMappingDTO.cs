using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class MobileGroupCodeWaybillMappingDTO : BaseDomainDTO
    {
        public int MobileGroupCodeWaybillMappingId { get; set; }
        public DateTime DateMapped { get; set; }

        public string GroupCodeNumber { get; set; }
        public string WaybillNumber { get; set; }

        public List<string> WaybillNumbers { get; set; }
        public List<PreShipmentMobileDTO> PreShipmentMobile { get; set; }
    }
}
