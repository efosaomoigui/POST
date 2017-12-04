namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class HaulagePricingDTO
    {
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public decimal WeightInTonne { get; set; }
    }
}
