using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.Enums;

namespace GIGL.GIGLS.Core.Domain
{
    public class FleetJobCard : BaseDomain
    {
        [Key]
        public int FleetJobCardId { get; set; }

        [MaxLength(12)]
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public string Status { get; set; }

        //public int FleetId { get; set; }
        //[ForeignKey("FleetId")]
        //public virtual Fleet Fleet { get; set; }

        public string FleetManagerId { get; set; }
        [ForeignKey("FleetManagerId")]
        public virtual Partner FleetManager { get; set; }

        public string EnterprisePartnerId { get; set; }
        [ForeignKey("EnterprisePartnerId")]
        public virtual FleetPartner EnterprisePartner { get; set; }
    }
}