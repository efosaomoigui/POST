using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain.Wallet
{
    public class CashOnDeliveryAccount : BaseDomain, IAuditable
    {
        public int CashOnDeliveryAccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public string UserId { get; set; }
        public CODStatus CODStatus { get; set; }
    }

    public class CashOnDeliveryRegisterAccount : BaseDomain, IAuditable
    {
        public int CashOnDeliveryRegisterAccountId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string Waybill { get; set; }
        public CODStatushistory CODStatusHistory { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenterCode { get; set; }
        public string Description { get; set; }
        public PaymentType PaymentType { get; set; }
        public int DepositStatus { get; set; }
    }
}
