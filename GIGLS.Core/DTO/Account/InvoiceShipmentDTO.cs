using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Account
{
    public class InvoiceShipmentDTO : BaseDomainDTO
    {
        public int InvoiceShipmentId { get; set; }
        public int InvoiceId { get; set; }
        public int ShipmentId { get; set; }
    }
}
