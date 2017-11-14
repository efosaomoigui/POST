using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetModelDTO : BaseDomainDTO
    {
        public FleetModelDTO()
        {
            Fleets = new List<FleetDTO>();
        }
        public int ModelId { get; set; }
        public string ModelName { get; set; }

        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public List<FleetDTO> Fleets { get; set; }
    }
}
