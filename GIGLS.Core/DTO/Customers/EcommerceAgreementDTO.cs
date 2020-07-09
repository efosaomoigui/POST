using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Customers
{
    public class EcommerceAgreementDTO : BaseDomainDTO
    {
        public int EcommerceAgreementId { get; set; }
        public string BusinessEmail { get; set; }
        public string BusinessOwnerName { get; set; }
        public string OfficeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ReturnAddress { get; set; }
        //public string EcommerceSignature { get; set; }
        public EcommerceSignatureDTO EcommerceSignature { get; set; }
        public List<string> NatureOfBusiness { get; set; }
    }

    public class EcommerceSignatureDTO 
    {
        public string EcommerceSignatureName { get; set; }
        public string EcommerceSignatureAddress { get; set; }
    }

}
