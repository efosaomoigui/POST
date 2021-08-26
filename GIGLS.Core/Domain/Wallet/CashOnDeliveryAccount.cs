using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

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

        [MaxLength(128)]
        public string UserId { get; set; }
        public CODStatus CODStatus { get; set; }

        public int CountryId { get; set; }
        [MaxLength(100), MinLength(5)]
        public string Waybill { get; set; }
    }

    public class CashOnDeliveryRegisterAccount : BaseDomain, IAuditable
    { 
        public int CashOnDeliveryRegisterAccountId { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(100), MinLength(5)]
        public string Waybill { get; set; }
        public CODStatushistory CODStatusHistory { get; set; }
        public int ServiceCenterId { get; set; }

        [MaxLength(50)]
        public string ServiceCenterCode { get; set; }
        public string Description { get; set; }
        public PaymentType PaymentType { get; set; }

        [MaxLength(100)]
        public string PaymentTypeReference { get; set; }
        public DepositStatus DepositStatus { get; set; }

        [MaxLength(100)]
        public string RefCode { get; set; }
        public int DepartureServiceCenterId { get; set; }
        public int DestinationCountryId { get; set; }
    }

    public class DemurrageRegisterAccount : BaseDomain, IAuditable 
    {
        [Key]
        public int DemurrageAccountId { get; set; } 
        public decimal Amount { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(100), MinLength(5)]
        public string Waybill { get; set; }
        public CODStatushistory DEMStatusHistory { get; set; } 
        public int ServiceCenterId { get; set; }

        [MaxLength(50)]
        public string ServiceCenterCode { get; set; }
        public string Description { get; set; }
        public PaymentType PaymentType { get; set; }

        [MaxLength(100)]
        public string PaymentTypeReference { get; set; }
        public DepositStatus DepositStatus { get; set; }

        [MaxLength(100)]
        public string RefCode { get; set; }
        public int CountryId { get; set; }
    }
}
