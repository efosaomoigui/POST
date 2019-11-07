using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Partnership
{
    public class SummaryTransactionsDTO
    {
        public List<PartnerTransactionsDTO> Transactions { get; set; }
        public decimal WalletBalance { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
    }
}
