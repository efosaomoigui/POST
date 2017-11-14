using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetPartInventoryHistoryDTO : BaseDomainDTO
    {
        public int FleetPartInventoryHistoryId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Remark { get; set; }
        public InventoryType InventoryType { get; set; }
        public string PartName { get; set; }
        public int PartId { get; set; }
        public string Store { get; set; }
        public int StoreId { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public string MovedBy { get; set; }
        public string Vendor { get; set; }
    }
}
