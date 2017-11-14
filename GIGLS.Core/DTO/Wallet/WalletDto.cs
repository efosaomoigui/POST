using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletDTO : BaseDomainDTO
    {
        public int WalletId { get; set; }
        public string WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public string UserType { get; set; }
        public string WalletOwnerName { get; set; }
    }
}
