using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
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

        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }
        public string Waybill { get; set; }
        public string ClientNodeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeReference { get; set; }
    }
}
