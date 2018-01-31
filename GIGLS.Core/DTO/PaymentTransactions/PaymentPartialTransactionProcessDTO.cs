using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class PaymentPartialTransactionProcessDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public List<PaymentPartialTransactionDTO> PaymentPartialTransactionDTOs { get; set; }
    }
}
