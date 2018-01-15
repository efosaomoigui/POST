using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.ShipmentScan
{
    public class ScanStatusDTO : BaseDomainDTO
    {
        public int ScanStatusId { get; set; }
        public string Code { get; set; }
        public string Incident { get; set; }
        public string ScanReason { get; set; }
        public string Comment { get; set; }
    }
}
