using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Account
{
    public class InsuranceDTO : BaseDomainDTO
    {
        public int InsuranceId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
