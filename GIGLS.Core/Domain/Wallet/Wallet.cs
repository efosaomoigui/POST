using GIGLS.Core.Domain;
using System;

namespace GIGLS.Core.Domain.Wallet
{
    public class Wallet : BaseDomain, IAuditable
    {
        public int WalletId { get; set; }
        public string WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public string UserType { get; set; }
    }
}
