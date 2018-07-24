using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class PricingDTO : BaseDomainDTO
    {
        public int DepartureServiceCentreId { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public int DeliveryOptionId { get; set; }
        public List<int> DeliveryOptionIds { get; set; }
        public decimal Weight { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public int SpecialPackageId { get; set; }
        public bool IsInternationalDocument { get; set; }
    }
}
