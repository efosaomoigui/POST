using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.BankSettlement
{

    public class BankProcessingOrderForShipmentAndCOD : BaseDomain, IAuditable  
    {
        [Key]
        public int ProcessingOrderId { get; set; } 

        [MaxLength(100), MinLength(5)] 
        public string Waybill { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? CODAmount { get; set; }
        public decimal? DemurrageAmount { get; set; }

        [MaxLength(100)]
        public string RefCode { get; set; }
        public DepositType DepositType { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public int ServiceCenterId { get; set; }
        public string ServiceCenter { get; set; }
        public DepositStatus Status { get; set; }

        [MaxLength(128)]
        public string VerifiedBy { get; set; }
    }
    
    public class BankProcessingOrderCodes : BaseDomain, IAuditable
    {
        [Key]
        public int CodeId { get; set; }

        [MaxLength(100)]
        public string Code { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateAndTimeOfDeposit { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int ServiceCenter { get; set; }
        public string ScName { get; set; }
        public DepositType DepositType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DepositStatus Status { get; set; }
        public string BankName { get; set; }

        [MaxLength(128)]
        public string VerifiedBy { get; set; }

    }

    public class CodPayOutList : BaseDomain, IAuditable
    {
        [Key]
        public int CodPayOutId { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateAndTimeOfDeposit { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; } 
        public string Name { get; set; }
        public int ServiceCenter { get; set; }
        public string ScName { get; set; }
        public bool IsCODPaidOut { get; set; }

        [MaxLength(128)]
        public string VerifiedBy { get; set; }
    }
}
