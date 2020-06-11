using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Partnership
{
    public class PartnerTransactionsDTO : BaseDomainDTO
    {
        public int PartnerTransactionsID { get; set; }
        public string UserId { get; set; }
        public string Destination { get; set; }
        public string Departure { get; set; }
        public decimal AmountReceived { get; set; }
        public string Waybill { get; set; }
        public string MobileGroupCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrencySymbol { get; set; }

        public bool IsFromServiceCentre { get; set; }
    }
    public class FleetPartnerTransactionsDTO : BaseDomainDTO
    {
        public FleetPartnerTransactionsDTO()
        {
            PreShipment = new PreShipmentMobileDTO();
        }

        public int PartnerTransactionsID { get; set; }
        public string UserId { get; set; }
        public string Destination { get; set; }
        public string Departure { get; set; }
        public decimal AmountReceived { get; set; }
        public string Waybill { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrencySymbol { get; set; }

        public PreShipmentMobileDTO PreShipment { get; set; }
    }

    public class ExternalPartnerTransactionsPaymentDTO : BaseDomainDTO
    {
        public int PartnerTransactionsID { get; set; }
        public decimal Amount { get; set; }
        public string Waybill { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PartnerName { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PartnerType { get; set; }

        public string CurrencySymbol { get; set; }
        public string EnterprisePartner { get; set; }
        public int Trips { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int GIGGOServiceCenter { get; set; }
        public string UserId { get; set; }
        public string ProcessedBy { get; set; }
        public ShipmentCollectionFilterCriteria filterCriteria { get; set; }
    }

    public class ExternalPartnerTransactionsPaymentCacheDTO : BaseDomainDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PartnerCode { get; set; }
        public string Email { get; set; }
        public int Trips { get; set; }
        public string PartnerType { get; set; }

        public string CurrencySymbol { get; set; }
        public decimal AmountReceived { get; set; }

        public string EnterprisePartner { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

    }

    public class CreditPartnerTransactionsDTO : BaseDomainDTO
    {
        public string Email { get; set; }
        public decimal AmountReceived { get; set; }
        public string Waybill { get; set; }
    }
}
