using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class SpecialDomesticPackageDTO : BaseDomainDTO
    {
        public int SpecialDomesticPackageId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public decimal Weight { get; set; }
    }
}
