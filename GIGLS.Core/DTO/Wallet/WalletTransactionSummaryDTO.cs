using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletTransactionSummaryDTO : BaseDomainDTO
    {
        public string WalletOwnerName { get; set; }
        public string WalletNumber { get; set; }
        public List<WalletTransactionDTO> WalletTransactions { get; set; }
    }
}
