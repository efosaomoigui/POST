using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.InternationalShipmentDetails
{
    public class InternationalRequestReceiverDTO : BaseDomainDTO
    {
        public int IdInternationalRequestReceiverId { get; set; }
        public string CustomerId { get; set; }
        public string GenerateCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public List<InternationalRequestReceiverItemDTO> InternationalRequestReceiverItems { get; set; }
    }
}
