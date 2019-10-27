namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class ReroutePricingDTO
    {
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public string Waybill { get; set; }
        public int CountryId { get; set; }
    }
}
