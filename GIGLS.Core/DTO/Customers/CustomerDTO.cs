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
        public string ReturnOption { get; set; }
        public int ReturnServiceCentre { get; set; }
        public string ReturnAddress { get; set; }
        public bool isCodNeeded { get; set; }

        // IndividualCustomerDTO
        public int IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string PictureUrl { get; set; }
        public string PicData { get; set; }
        public string WalletNumber { get; set; }
        public decimal WalletBalance { get; set; }
        public string CustomerCode { get; set; }

        public string Password { get; set; }

        //User Active CountryId
        public int UserActiveCountryId { get; set; }
        public bool IsFromMobile { get; set; }
        public CountryDTO Country { get; set; }

        //Update for user table
        public bool IsInternational { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string IdentifictionNumber { get; set; }
        public string IdentificationImage { get; set; } 
        public string IdentificationNumber { get; set; }  

    }
}
