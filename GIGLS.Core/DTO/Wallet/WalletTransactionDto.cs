using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.User;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

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
        public int TransactionCountryId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ServiceCharge { get; set; }
        public UserDTO User { get; set; }
        public decimal TotalAmount { get; set; }
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
        public int TransactionCountryId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
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
        public decimal Cellulant { get; set; }
        public decimal Sterling { get; set; }
        public decimal Korapay { get; set; }
    }

    public class WalletBreakdown
    {
        public decimal IndividualCustomer { get; set; }
        public decimal Ecommerce { get; set; }
    }

    public class WalletCreditTransactionDTO
    {
        public DateTime DateOfEntry { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public UserDetailForCreditTransactionDTO User { get; set; }
    }

    public class WalletCreditTransactionSummaryDTO
    {
        public decimal NairaAmount { get; set; }
        public decimal CedisAmount { get; set; }
        public decimal PoundsAmount { get; set; }
        public decimal DollarAmount { get; set; }
        public List<WalletCreditTransactionDTO> WalletCreditTransactions { get; set; }
    }

    public class UserDetailForCreditTransactionDTO
    {
        public string CustomerCode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }

    public class ForexTransactionHistoryDTO
    {
        public DateTime DateOfEntry { get; set; }
        public decimal FundedAmount { get; set; }
        public decimal EquivalentAmount { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public CardType CardType { get; set; }
        public string EquivalentCurrencyCode { get; set; }
        public string EquivalentCurrencySymbol { get; set; }
        public OnlinePaymentType ProcessingPartner { get; set; }
        public UserDetailForCreditTransactionDTO User { get; set; }
    }
}
