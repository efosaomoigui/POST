using POST.CORE.DTO;

namespace POST.Core.DTO.BankSettlement
{
    public class BankDTO : BaseDomainDTO
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
    }
}
