using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Customers
{
    public class CompanyContactPersonDTO : BaseDomainDTO
    {
        public int CompanyContactPersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }

        //Company Details
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRcNumber { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyIndustry { get; set; }
        public CompanyType CompanyType { get; set; }
        public CompanyStatus CompanyStatus { get; set; }
        public decimal CompanyDiscount { get; set; }
        public DateTime CompanyDateCreated { get; set; }
        public DateTime CompanyDateModified { get; set; }
    }

    public class NewCompanyContactPersonDTO : BaseDomainDTO
    {
        public int CompanyContactPersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }
    }
}
