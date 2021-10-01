using GIGLS.Core.Enums;

namespace GIGLS.Core.DTO.DHL
{
    public class TotalNetResult
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal VAT { get; set; }
        public decimal Insurance { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Discount { get; set; }
        public CompanyMap CompanyMap { get; set; }
        public decimal InternationalShippingCost { get; set; }
        public string ShipmentMethod { get; set; } = "Express";
        public bool IsWithinProcessingTime { get; set; }
    }
}
