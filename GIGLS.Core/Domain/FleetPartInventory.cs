using GIGLS.Core.Domain;

namespace GIGL.GIGLS.Core.Domain
{
    public class FleetPartInventory : BaseDomain
    {
        public int FleetPartInventoryId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int PartId { get; set; }
        public virtual FleetPart FleetPart { get; set; }

        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}