using POST.CORE.DTO;
using System.Collections.Generic;

namespace POST.Core.DTO.PaymentTransactions
{
    public class PaymentPartialTransactionProcessDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public List<PaymentPartialTransactionDTO> PaymentPartialTransactions { get; set; }
    }
}
