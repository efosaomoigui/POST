using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.JobCards
{
    public class JobCardManagementPartDTO : BaseDomainDTO
    {
        public int JobCardManagementPartId { get; set; }
        public int Quantity { get; set; }
        public string FleetPart { get; set; }
        public string JobCardManagement { get; set; }
    }
}
