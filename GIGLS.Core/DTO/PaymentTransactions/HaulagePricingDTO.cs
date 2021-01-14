namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class HaulagePricingDTO
    {
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public int Haulageid { get; set; }
        public int CountryId { get; set; }
        public string CustomerCode { get; set; }
    }
    public class HaulagePriceDTO
    {
        public int DepartureStationId { get; set; }
        public int DestinationStationId { get; set; }
        public int Haulageid { get; set; }
       
    }
}
