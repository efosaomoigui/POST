using POST.CORE.DTO;

namespace POST.Core.DTO.Wallet
{
    public class WalletNumberDTO : BaseDomainDTO
    {
        public int WalletNumberId { get; set; }
        public string WalletPan { get; set; }
        public bool IsActive { get; set; }
    }
}
