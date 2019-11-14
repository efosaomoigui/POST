using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain
{
    public class SpecialDomesticPackage : BaseDomain, IAuditable
    {
        public int SpecialDomesticPackageId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public decimal Weight { get; set; }
        public SpecialDomesticPackageType SpecialDomesticPackageType { get; set; }

        //new properties added for categorization
        public virtual SubCategory SubCategory { get; set; }

        public string WeightRange { get; set; }

        
    }
}
