using GIGLS.Core;
using GIGLS.Core.Enums;
using GIGLS.Core.Domain;
using System.Collections.Generic;

namespace GIGL.GIGLS.Core.Domain
{
    public class Company : BaseDomain, IAuditable
    {
        public Company()
        {
            CompanyContactPersons = new HashSet<CompanyContactPerson>();
        }
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
        public int SettlementPeriod { get; set; }
        public virtual ICollection<CompanyContactPerson> CompanyContactPersons { get; set; }


    }
}