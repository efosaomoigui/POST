using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.Domain
{
    public class GIGGOCODTransfer : BaseDomain
    {
        public string GIGGOCODTransferID { get; set; } = Guid.NewGuid().ToString();
        public string Waybill { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Amount { get; set; }
        public string BankName { get; set; }
    }
}
