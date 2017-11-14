using GIGLS.Core.Domain;
using System;

namespace GIGLS.Core.Domain.Wallet
{
    public class WalletNumber :BaseDomain, IAuditable
    {
        public int WalletNumberId { get; set; }
        public string WalletPan { get; set; }
        public bool IsActive { get; set; }
    }
}
