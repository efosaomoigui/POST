using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class CODWalletDTO : BaseDomainDTO
    {
        public int CODWalletId { get; set; }
        public string AccountNo { get; set; }
        public decimal AvailableBalance { get; set; }
        public int CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyType { get; set; }
        public string AccountType { get; set; }
        public decimal WithdrawableBalance { get; set; }
        public string CustomerAccountId { get; set; }
    }

    public class CreateStellaAccountDTO 
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string otherNames { get; set; }
        public string bvn { get; set; }
        public string phoneNo { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string CustomerCode { get; set; }

    }
}
