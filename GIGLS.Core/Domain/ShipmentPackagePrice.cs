using System.ComponentModel.DataAnnotations;

namespace POST.Core.Domain
{
    public class ShipmentPackagePrice : BaseDomain, IAuditable
    {
        public int ShipmentPackagePriceId { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CountryId { get; set; }
        
        public int InventoryOnHand { get; set; }
        public int MinimunRequired { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Weight { get; set; }
    }
}