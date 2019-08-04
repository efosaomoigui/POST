using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.BankSettlement
{
    public class CODSettlementSheet : BaseDomain, IAuditable
    {
        [Key]
        public int CODSettlementSheetId { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Waybill { get; set; }

        [MaxLength(100)]
        public string ReceiverAgentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DateSettled { get; set; }

        [MaxLength(100)]
        public string CollectionAgentId { get; set; }
        public bool ReceivedCOD { get; set; }
    }     
}