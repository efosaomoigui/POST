using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain
{
    public class VAT : BaseDomain, IAuditable
    {
        public int VATId { get; set; }
        public string Name { get; set; }
        public VATType Type { get; set; }
        public decimal Value { get; set; }
        public int CountryId { get; set; }
    }
}