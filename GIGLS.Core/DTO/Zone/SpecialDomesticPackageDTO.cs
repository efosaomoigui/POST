using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Zone
{
    public class SpecialDomesticPackageDTO : BaseDomainDTO
    {
        public int SpecialDomesticPackageId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public decimal Weight { get; set; }
        public SpecialDomesticPackageType SpecialDomesticPackageType { get; set; }


        //new properties added for categorization
        public SubCategoryDTO SubCategory { get; set; }

    }


    public class SpecialResultDTO : BaseDomainDTO
    {
        public List<SpecialDomesticPackageDTO> Specialpackages { get; set; }
        public List<CategoryDTO> Categories { get; set; }

        public List<SubCategoryDTO> SubCategories { get; set; }

    }
}
