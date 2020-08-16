﻿using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Customers
{
    public class CompanyDTO : BaseDomainDTO
    {
        public CompanyDTO()
        {
            ContactPersons = new List<CompanyContactPersonDTO>();
            CompanyShipments = new List<ShipmentDTO>();
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
        public string CustomerCode { get; set; }
        public CustomerCategory CustomerCategory { get; set; }
        
        public string ReturnOption { get; set; }
        public int ReturnServiceCentre { get; set; }
        public string ReturnServiceCentreName { get; set; }
        public string ReturnAddress { get; set; }
        public decimal WalletBalance { get; set; }

        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<CompanyContactPersonDTO> ContactPersons { get; set; }
        public List<ShipmentDTO> CompanyShipments { get; set; }

        //User Active CountryId
        public int UserActiveCountryId { get; set; }
        public string UserActiveCountryName { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsFromMobile { get; set; }
        public bool IsRegisteredFromMobile { get; set; }

        public CountryDTO Country { get; set; }
        public bool isCodNeeded { get; set; }

        //added this for Giglgo customers
        public decimal? WalletAmount { get; set; }
        public bool? IsEligible { get; set; }

        //For EcommerceAgreement
        public int EcommerceAgreementId { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

    }
}
