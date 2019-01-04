using GIGLS.Core.DTO.Account;

namespace GIGLS.CORE.DTO.Shipments
{
    public class ShipmentReturnDTO : BaseDomainDTO
    {
        public string WaybillNew { get; set; }
        public string WaybillOld { get; set; }
        public decimal Discount { get; set; }
        public decimal OriginalPayment { get; set; }
        public int ServiceCentreId { get; set; }
        public ServiceCenreDTO ServiceCentre { get; set; }
    }
}
