using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.InternationalShipmentDetails
{
     public class InternationalRequestReceiverItemDTO : BaseDomainDTO
    {
        public int InternationalRequestReceiverItemId { get; set; }
        public int InternationalRequestReceiverId { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string Lenght { get; set; }
        public string Height { get; set; }
        public string Value { get; set; }
    }
}
