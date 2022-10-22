using POST.CORE.DTO;

namespace POST.Core.DTO.Account
{
    public class InsuranceDTO : BaseDomainDTO
    {
        public int InsuranceId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }
    }
}
