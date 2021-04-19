namespace GIGLS.Core.DTO.Dashboard
{
    public class EarningsBreakdownDTO
    {
        public decimal GIGGO { get; set; }
        public decimal Agility { get; set; }
        public decimal IntlShipments { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Demurrage { get; set; }
    }

    public class EarningsBreakdownByCustomerDTO
    {
        public decimal Individual { get; set; }
        public decimal Ecommerce { get; set; }
        public decimal Corporate { get; set; }
    }

    public class EarningsBreakdownOfEcommerceDTO
    {
        public decimal Basic { get; set; }
        public decimal Class { get; set; }
    }
}
