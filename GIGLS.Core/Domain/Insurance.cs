namespace GIGLS.Core.Domain
{
    public class Insurance : BaseDomain, IAuditable
    {
        public int InsuranceId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int CountryId { get; set; }
    }
}