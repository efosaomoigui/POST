using POST.CORE.DTO;
using System;

namespace POST.Core.DTO.Fleets
{
    public class FleetLocationDTO : BaseDomainDTO
    {
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
    }
}