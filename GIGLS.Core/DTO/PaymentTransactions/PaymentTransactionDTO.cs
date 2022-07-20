using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class PaymentTransactionDTO : BaseDomainDTO
    {
        public int PaymentTransactionId { get; set; }
        public string Waybill { get; set; }
        public string TransactionCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentType { get; set; }
        public string UserId { get; set; }
        public bool FromApp { get; set; }
        public bool IsNotOwner { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerUserId { get; set; }
        public List<WaybillDTO> Waybills { get; set; }
        public List<TransactionCodeDTO> TransactionCodes { get; set; }
    }

    public class WaybillDTO
    {
        public string Waybill { get; set; }
    }

    public class TransactionCodeDTO
    {
        public string TransactionCode { get; set; }
    }
}
