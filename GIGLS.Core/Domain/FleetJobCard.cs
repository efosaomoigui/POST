using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using POST.Core.Domain;
using POST.Core.Domain.Partnership;
using POST.Core.Enums;

namespace GIGL.POST.Core.Domain
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
        public string PaymentReceiptUrl { get; set; }

        public int FleetId { get; set; }
        [ForeignKey("FleetId")]
        public virtual Fleet Fleet { get; set; }

        public string FleetManagerId { get; set; }

        public string FleetOwnerId { get; set; }
        [ForeignKey("FleetOwnerId")]
        public virtual User FleetOwner { get; set; }
    }
}