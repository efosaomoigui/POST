using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletTransactionDTO : BaseDomainDTO
    {
        public int WalletTransactionId { get; set; }
        public int WalletId { get; set; }
        public int TransactionType { get; set; }
        public int TransactionSourceId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UserType { get; set; }
        public string WalletOwnerName { get; set; }
        public decimal Balance { get; set; }
        public string WalletNumber { get; set; }
        public decimal LineBalance { get; set; }
    }

    public class WalletTransactionSummaryDTO : BaseDomainDTO
    {
        public string WalletOwnerName { get; set; }
        public string WalletNumber { get; set; }
        public List<WalletTransactionDTO> WalletTransactions { get; set; }
    }
}
