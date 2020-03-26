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
}
