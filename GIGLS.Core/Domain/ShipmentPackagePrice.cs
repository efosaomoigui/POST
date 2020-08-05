namespace GIGLS.Core.Domain
{
    public class ShipmentPackagePrice : BaseDomain, IAuditable
    {
        public int ShipmentPackagePriceId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CountryId { get; set; }
        public int Balance { get; set; }
    }
}