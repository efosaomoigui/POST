using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.Domain.Wallet
{
    public class AccountTransaction : BaseDomain, IAuditable
    {
        public int AccountTransactionId { get; set; }

        public double Amount { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string Narration { get; set; }
        public string TransactionReference { get; set; }
        public virtual AccountType AccountType { get; set; }
    }
}
