using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetPartInventoryDTO : BaseDomainDTO
    {
        public int FleetPartInventoryId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string PartName { get; set; }
        public int PartId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }
}
