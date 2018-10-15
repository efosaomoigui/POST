namespace GIGLS.Core.Domain
{
    public class SpecialDomesticPackage : BaseDomain, IAuditable
    {
        public int SpecialDomesticPackageId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public decimal Weight { get; set; }
    }
}
