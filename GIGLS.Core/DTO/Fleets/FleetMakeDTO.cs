using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetMakeDTO : BaseDomainDTO
    {
        public FleetMakeDTO()
        {
            FleetModels = new List<FleetModelDTO>();
            Fleets = new List<FleetDTO>();
        }
        public int MakeId { get; set; }
        public string MakeName { get; set; }

        public List<FleetDTO> Fleets { get; set; }
        public List<FleetModelDTO> FleetModels { get; set; }
    }
}
