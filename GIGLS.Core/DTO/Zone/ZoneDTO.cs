using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Zone
{
    public class ZoneDTO : BaseDomainDTO
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public bool Status { get; set; }
    }
}
