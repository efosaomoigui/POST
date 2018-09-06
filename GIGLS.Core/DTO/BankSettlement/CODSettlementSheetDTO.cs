using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.BankSettlement
{
    public class CODSettlementSheetDTO : BaseDomainDTO
    {
        public int CODSettlementSheetId { get; set; }
        public string ReceiverAgentId { get; set; }
        public string ReceiverAgent { get; set; }
        public string Waybill { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DateSettled { get; set; }
        public string CollectionAgentId { get; set; }
        public string CollectionAgent { get; set; }
        public bool ReceivedCOD { get; set; }
    }
}
