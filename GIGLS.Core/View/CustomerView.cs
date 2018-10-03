using GIGLS.Core.DTO.Customers;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.View
{
    public class CustomerView : BaseDomainDTO
    {
        [Key]
        public string CustomerCode { get; set; }

        //company
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string RcNumber { get; set; }

        public string Industry { get; set; }
        public CompanyType? CompanyType { get; set; }
        public CompanyStatus? CompanyStatus { get; set; }
        public decimal? Discount { get; set; } = 0;
        public int? SettlementPeriod { get; set; } = 0;

        //public string ReturnOption { get; set; }
        //public int ReturnServiceCentre { get; set; }
        //public string ReturnAddress { get; set; }


        //individual
        public int? IndividualCustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string PictureUrl { get; set; }
        

        //common properties
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}
