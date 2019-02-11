using GIGLS.Core.Enums;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using System.ComponentModel.DataAnnotations;
using GIGLS.Core.Domain;

namespace GIGL.GIGLS.Core.Domain
{
    public class Fleet : BaseDomain, IAuditable
    {
        [Key]
        public int FleetId { get; set; }
        public string RegistrationNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string EngineNumber { get; set; }
        public bool Status { get; set; }
        public FleetType FleetType { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }

        public int ModelId { get; set; }
        public virtual FleetModel FleetModel { get; set; }

        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }

    }
}