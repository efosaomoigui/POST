using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class RiderDeliveryViewDTO : BaseDomainDTO
    {        
        public decimal Total { get; set; }
              
        public List<RiderDeliveryDTO> RiderDelivery { get; set; }
    }
}
