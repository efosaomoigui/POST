namespace GIGLS.Core.Domain
{
    public class Country : BaseDomain, IAuditable
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}
