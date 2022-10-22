using POST.CORE.DTO;
using System.Collections.Generic;

namespace POST.Core.DTO
{
    public class RiderDeliveryViewDTO : BaseDomainDTO
    {        
        public decimal Total { get; set; }              
        public List<RiderDeliveryDTO> RiderDelivery { get; set; }
    }
}