using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletTransactionDTO : BaseDomainDTO
    {
        public int WalletTransactionId { get; set; }

        public DateTime DateOfEntry { get; set; }

        public int ServiceCentreId { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }

        public int WalletId { get; set; }
        public WalletDTO Wallet { get; set; }

        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }
        public string Waybill { get; set; }
        public string Manifest { get; set; }
        public string ClientNodeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeReference { get; set; }
        public string PassKey { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
    }

    public class ModifiedWalletTransactionDTO
    {
        public int WalletTransactionId { get; set; }
        public DateTime DateOfEntry { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }
        public string Waybill { get; set; }
        public PaymentType PaymentType { get; set; }
    }

    public class WalletTransactionSummary
    {
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
    }

    public class WalletPaymentLogSummary
    {
        public decimal Paystack { get; set; }
        public decimal TheTeller { get; set; }
        public decimal Flutterwave { get; set; }
        public decimal USSD { get; set; }
    }

    public class WalletBreakdown
    {
        public decimal IndividualCustomer { get; set; }
        public decimal Ecommerce { get; set; }
    }
}
