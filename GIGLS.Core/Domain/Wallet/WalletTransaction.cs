using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class WalletTransaction : BaseDomain
    {
        [Key]
        public int WalletTransactionId { get; set; }

        public DateTime DateOfEntry { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public string ClientNodeId { get; set; }
        public PaymentType PaymentType { get; set; }

        [MaxLength(100)]
        public string PaymentTypeReference { get; set; }
        public decimal BalanceAfterTransaction { get; set; }

        [MaxLength(100)]
        public string Manifest { get; set; }
        public int TransactionCountryId { get; set; }
    }
}
