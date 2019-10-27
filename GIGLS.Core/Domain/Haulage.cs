namespace GIGLS.Core.Domain
{
    public class Haulage : BaseDomain, IAuditable
    {
        public int HaulageId { get; set; }
        public decimal Tonne { get; set; }
        public string Description { get; set; } 
        public bool Status { get; set; }
        public decimal FixedRate { get; set; }
        public decimal AdditionalRate { get; set; }
        public int CountryId { get; set; }
    }
}