using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GIGLS.Core.DTO.BankSettlement
{
 
    public class BankProcessingOrderForShipmentAndCODDTO : BaseDomain, IAuditable
    {
        [Key]
        public int ProcessingOrderId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }
        public decimal GrandTotal { get; set; } 
        public decimal CODAmount { get; set; }
        public decimal DemurrageAmount { get; set; }
        public string RefCode { get; set; }
        public DepositType DepositType { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }

        public int ServiceCenterId { get; set; }
        public string ServiceCenter { get; set; }
        public DepositStatus Status { get; set; }
        public string VerifiedBy { get; set; }
    }

    public class BankProcessingOrderCodesDTO : BaseDomain, IAuditable
    {
        [Key]
        public int CodeId { get; set; }
        public string Code { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateAndTimeOfDeposit { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; } 
        public int ServiceCenter { get; set; }
        public string ScName { get; set; } 
        public DepositType DepositType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DepositStatus Status { get; set; }
        public string VerifiedBy { get; set; }
        public string BankName { get; set; }
        //Shipment Items
        public List<BankProcessingOrderForShipmentAndCODDTO> ShipmentAndCOD { get; set; }

        ////COD Items
        //public List<CashOnDeliveryRegisterAccount> CODRegister { get; set; }

        ////Demurrage Items
        //public List<DemurrageRegisterAccount> DemurrageRegister { get; set; }

    }

    public class CodPayOutListDTO : BaseDomain, IAuditable 
    {
        [Key]
        public int CodPayOutId { get; set; }
        public string Waybill { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DateAndTimeOfDeposit { get; set; }
        public string UserId { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public int ServiceCenter { get; set; }
        public string ScName { get; set; }
        public DepositType IsCODPaidOut { get; set; }
        public string VerifiedBy { get; set; }
    }

    public class CombiineBankOrDerDTO : BaseDomain, IAuditable  
    {
        //public BankProcessingOrderForShipmentAndCODDTO f =  new BankProcessingOrderForShipmentAndCODDTO();
        public BankProcessingOrderCodesDTO orderval = new BankProcessingOrderCodesDTO();
        public decimal CODAmount { get; set; } 
    }
}
