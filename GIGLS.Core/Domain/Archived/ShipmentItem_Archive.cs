using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Archived
{
    public class ShipmentItem_Archive : BaseDomain_Archive
    {
        [Key]
        public int ShipmentItemId { get; set; }
        public string Description { get; set; }
        public string Description_s { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public double Weight { get; set; }
        public string Nature { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int SerialNumber { get; set; }
        public int ShipmentPackagePriceId { get; set; }
        public int PackageQuantity { get; set; }
        public bool IsVolumetric { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int ShipmentId { get; set; }
    }
}
