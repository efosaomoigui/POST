using System;

namespace GIGLS.Core.Domain
{
    public class Zone : BaseDomain, IAuditable
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public bool Status { get; set; }
    }
}
