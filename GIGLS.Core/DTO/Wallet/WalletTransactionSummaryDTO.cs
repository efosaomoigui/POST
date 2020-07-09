using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletTransactionSummaryDTO : BaseDomainDTO
    {
        public int WalletId { get; set; }
        public string WalletOwnerName { get; set; }
        public string WalletNumber { get; set; }
        public decimal WalletBalance { get; set; }
        public List<WalletTransactionDTO> WalletTransactions { get; set; }
        public List<TransactionPreShipmentDTO> NewShipments { get; set; }
        public List<PreShipmentMobileDTO> Shipments { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
    }
    public class ModifiedWalletTransactionSummaryDTO : BaseDomainDTO
    {
        public int WalletId { get; set; }
        public string WalletOwnerName { get; set; }
        public string WalletNumber { get; set; }
        public decimal WalletBalance { get; set; }
        public List<WalletTransactionDTO> WalletTransactions { get; set; }
        public List<TransactionPreShipmentDTO> Shipments { get; set; }
        public List<string> Status { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
    }
}
