using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Shipments
{
    public class MissingShipmentDTO : BaseDomainDTO
    {
        public int MissingShipmentId { get; set; }
        public string Waybill { get; set; }
        public double SettlementAmount { get; set; }
        public string Comment { get; set; }
    }
}
