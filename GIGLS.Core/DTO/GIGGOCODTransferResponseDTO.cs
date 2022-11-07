using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class GIGGOCODTransferResponseDTO : BaseDomainDTO
    {
        public string GIGGOCODTransferID { get; set; }
        public string Waybill { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Amount { get; set; }
        public string BankName { get; set; }
    }
}
