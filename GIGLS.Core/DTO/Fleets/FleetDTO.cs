using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetDTO : BaseDomainDTO
    {
        public int FleetId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string EngineNumber { get; set; }        
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public FleetType FleetType { get; set; }
        public int Capacity { get; set; }
    }
}
