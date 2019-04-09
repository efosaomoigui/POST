using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class CashOnDeliveryAccountDTO : BaseDomainDTO
    {
        public int CashOnDeliveryAccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public int WalletId { get; set; }
        public WalletDTO Wallet { get; set; }
        public string UserId { get; set; }
        public string Waybill { get; set; }
        public CODStatus CODStatus { get; set; }
    }

    public class CashOnDeliveryRegisterAccountDTO : BaseDomainDTO  
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
        public DepositStatus DepositStatus { get; set; }
        public string RefCode { get; set; }
        public int DepartureServiceCenterId { get; set; }
    }

    public class DemurrageRegisterAccountDTO : BaseDomainDTO 
    {
        public int DemurrageAccountId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string Waybill { get; set; }
        public CODStatushistory DEMStatusHistory { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenterCode { get; set; }
        public string Description { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeReference { get; set; }
        public DepositStatus DepositStatus { get; set; }
        public string RefCode { get; set; }
    }
}
