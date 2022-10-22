using POST.Core.Enums;

namespace POST.Core.Domain.Wallet
{
    public class AccountSummary : BaseDomain, IAuditable
    {
        public int AccountSummaryId { get; set; }

        public double Balance { get; set; }
        public virtual AccountType AccountType { get; set; }
    }
}
