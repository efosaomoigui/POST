using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.View
{
    public class WalletPaymentLogView : BaseDomainDTO
    {
        [Key]
        public int WalletPaymentLogId { get; set; }
        public int WalletId { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string UserId { get; set; }
        public bool IsWalletCredited { get; set; }

        //Wallet
        public string WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerCode { get; set; }
        public string CompanyType { get; set; }

        //CustomerView
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public int? IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //CystomerName
        public string CustomerName
        {
            get
            {
                if (CustomerType.Equals(CustomerType.Company))
                {
                    return Name;
                }
                else
                {
                    return string.Format($"{FirstName} {LastName}");
                }
            }
        }
    }
}
