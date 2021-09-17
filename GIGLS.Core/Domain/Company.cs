using GIGLS.Core;
using GIGLS.Core.Enums;
using GIGLS.Core.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GIGL.GIGLS.Core.Domain
{
    public class Company : BaseDomain, IAuditable
    {
        public Company()
        {
            CompanyContactPersons = new HashSet<CompanyContactPerson>();
        }
        public int CompanyId { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string RcNumber { get; set; }

        [MaxLength(500)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        //User Active CountryId
        public int UserActiveCountryId { get; set; }

        [MaxLength(20), MinLength(3)]
        [Index(IsUnique = true)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Industry { get; set; }
        public CompanyType CompanyType { get; set; }
        public CompanyStatus CompanyStatus { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }
        public decimal Discount { get; set; }
        public int SettlementPeriod { get; set; }
        public virtual ICollection<CompanyContactPerson> CompanyContactPersons { get; set; }
        public CustomerCategory CustomerCategory { get; set; }

        [MaxLength(100)]
        public string ReturnOption { get; set; }
        public int ReturnServiceCentre { get; set; }

        [MaxLength(500)]
        public string ReturnAddress { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        public bool IsRegisteredFromMobile { get; set; }
        public bool isCodNeeded { get; set; }

        //added this for Giglgo customers
        public decimal? WalletAmount { get; set; }
        public bool? IsEligible { get; set; }

        [MaxLength(500)]
        public string AccountName { get; set; }

        [MaxLength(100)]
        public string AccountNumber { get; set; }
        public string ProductType { get; set; }

        [MaxLength(100)]
        public string BankName { get; set; }
        public Rank Rank { get; set; }

        [MaxLength(50)]
        public string BVN { get; set; }
        public string AssignedCustomerRep { get; set; }
        [MaxLength(500)]
        public string IdentificationNumber { get; set; }                                                                                                                                        

        [MaxLength(500)]
        public string IdentificationImageUrl { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public bool IsInternational { get; set; }
        public DateTime RankModificationDate { get; set; }
        public WalletTransactionType TransactionType { get; set; }

        [MaxLength(100)]
        public string NUBANAccountNo { get; set; }

        [MaxLength(128)]
        public string PrefferedNubanBank { get; set; }
        public int NUBANCustomerId { get; set; }
        [MaxLength(128)]
        public string NUBANCustomerCode { get; set; }
        [MaxLength(300)]
        public string NUBANCustomerName { get; set; }

    }
}