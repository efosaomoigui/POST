using GIGLS.Core.Domain;
using System;

namespace GIGLS.Core.Domain.Wallet
{
    public class GeneralLedger: BaseDomain
    {
        public int GeneralLedgerId { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int TransactionTypeId { get; set; }
        public int TransactionSourceId { get; set; }
    }
}
