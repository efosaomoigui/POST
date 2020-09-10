using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class ServiceCenterPackage : BaseDomain, IAuditable
    {
        [Key]
        public int ServiceCenterPackageId { get; set; }
        public int ShipmentPackageId { get; set; }
        public int ServiceCenterId { get; set; }
        public int InventoryOnHand { get; set; }
        public int MinimunRequired { get; set; }
    }

}
