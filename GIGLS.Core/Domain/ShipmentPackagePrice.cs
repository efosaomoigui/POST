namespace GIGLS.Core.Domain
{
    public class ShipmentPackagePrice : BaseDomain, IAuditable
    {
        public int ShipmentPackagePriceId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
