using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Vendors
{
    public class VendorDTO : BaseDomainDTO
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string VendorType { get; set; }
    }
}
