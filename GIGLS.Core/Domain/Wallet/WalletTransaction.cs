using GIGLS.Core.Domain;
using System;

namespace GIGLS.Core.Domain.Wallet
{
    public class WalletTransaction : BaseDomain
    {
        public int WalletTransactionId { get; set; }
        public int WalletId { get; set; }
        public int TransactionType { get; set; }
        public int TransactionSourceId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal LineBalance { get; set; }
    }
}
