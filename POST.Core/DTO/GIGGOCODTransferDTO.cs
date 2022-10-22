using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class GIGGOCODTransferDTO 
    {
        public string Waybill { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string BankName { get; set; }
    }
}
