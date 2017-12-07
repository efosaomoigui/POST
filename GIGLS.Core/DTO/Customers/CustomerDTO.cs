using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Customers
{
    public class CustomerDTO : BaseDomainDTO
    {
        //CustomerDTO
        public CustomerType CustomerType { get; set; }
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

        // CompanyDTO
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string RcNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Industry { get; set; }
        public CompanyType CompanyType { get; set; }
        public CompanyStatus CompanyStatus { get; set; }
        public decimal Discount { get; set; }

        // IndividualCustomerDTO
        public int IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        //public string Email { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Address { get; set; }
        //public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string PicData { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime DateModified { get; set; }
        public string WalletNumber { get; set; }
    }
}
