using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO
{
    public class RiderDeliveryViewDTO : BaseDomainDTO
    {        
        public decimal Total { get; set; }              
        public List<RiderDeliveryDTO> RiderDelivery { get; set; }
    }
}