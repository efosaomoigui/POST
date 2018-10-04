using GIGLS.Core.DTO.Shipments;
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

        public List<CompanyContactPersonDTO> ContactPersons { get; set; }
        public List<ShipmentDTO> CompanyShipments { get; set; }
    }
}
