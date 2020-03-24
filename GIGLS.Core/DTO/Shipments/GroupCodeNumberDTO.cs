using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class GroupCodeNumberDTO : BaseDomainDTO
    {
        public int GroupCodeNumberId { get; set; }
       
        public string GroupCode { get; set; }
        public bool IsActive { get; set; }
    }
}
