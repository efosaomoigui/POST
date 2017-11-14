using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetLocationDTO : BaseDomainDTO
    {
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
    }
}