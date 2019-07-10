using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Partnership
{
    public class PartnerTransactionsDTO : BaseDomainDTO
    {
        public int PartnerTransactionsID { get; set; }
        public string UserId { get; set; }
        public string Destination { get; set; }
        public string Departure { get; set; }
        public decimal AmountReceived { get; set; }
    }
}
