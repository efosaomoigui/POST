using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class FleetPartInventoryHistory : BaseDomain, IAuditable
    {
        public int FleetPartInventoryHistoryId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Remark { get; set; }
        public InventoryType InventoryType { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }

        public int PartId { get; set; }
        public FleetPart FleetPart { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }
        
        public virtual User MovedBy { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}