using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class CODCustomerAccountStatementDto
    {
        public decimal AmountPaid { get; set; }
        public string SourceAccountNumber { get; set; }
        public string SourceBankName { get; set; }
        public string DestinationAccountNumber { get; set; }
        public string DestinationBankName { get; set; }
        public string TransactionType { get; set; }
        public string Narration { get; set; }
        public DateTime DatePaid { get; set; }
    }
}
