using POST.Core.DTO.ServiceCentres;
using POST.CORE.DTO;

namespace POST.Core.DTO.Shipments
{
    public class MissingShipmentDTO : BaseDomainDTO
    {
        public int MissingShipmentId { get; set; }
        public string Waybill { get; set; }
        public string Comment { get; set; }
        public string Reason { get; set; }
        public double SettlementAmount { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public string CreatedBy { get; set; }
        public string ResolvedBy { get; set; }
        public int ServiceCentreId { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }
    }
}
