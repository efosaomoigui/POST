using POST.CORE.DTO;

namespace POST.Core.DTO.Zone
{
    public class ZoneDTO : BaseDomainDTO
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public bool Status { get; set; }
        public decimal ZonePercent { get; set; }
    }
}
