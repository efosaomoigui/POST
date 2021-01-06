namespace GIGLS.Core.DTO.DHL
{
    public class AddressPayload
    {
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string StreetLines { get; set; }
        public string StreetLines2 { get; set; }
        public string StreetLines3 { get; set; }
        public string StateOrProvinceCode { get; set; }
    }
}
