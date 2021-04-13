using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class Zone : BaseDomain, IAuditable
    {
        public int ZoneId { get; set; }

        [MaxLength(100)]
        public string ZoneName { get; set; }
        public bool Status { get; set; }
    }
}
