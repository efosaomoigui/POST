using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class WalletNumber :BaseDomain, IAuditable
    {
        public int WalletNumberId { get; set; }

        [MaxLength(100)]
        public string WalletPan { get; set; }
        public bool IsActive { get; set; }
    }
}
