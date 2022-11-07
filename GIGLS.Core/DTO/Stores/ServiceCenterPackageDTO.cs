using POST.CORE.DTO;

namespace POST.Core.DTO.Stores
{
    public class ServiceCenterPackageDTO : BaseDomainDTO
    {
        public int ShipmentPackageId { get; set; }
        public string ShipmentPackageName { get; set; }
        public int ServiceCenterId { get; set; }
        public int InventoryOnHand { get; set; }
        public int MinimunRequired { get; set; }
    }
}
