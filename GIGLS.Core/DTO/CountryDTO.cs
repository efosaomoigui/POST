using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO
{
    public class CountryDTO : BaseDomainDTO
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
}
