using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;

namespace POST.Core.Domain
{
    public class FleetDisputeMessage : BaseDomain, IAuditable
    {
        [Key]
        public int FleetDisputeMessageId { get; set; }

        public string VehicleNumber { get; set; }
        public string DisputeMessage { get; set; }

        public string FleetOwnerId { get; set; }
        [ForeignKey("FleetOwnerId")]
        public virtual User FleetOwner { get; set; }
    }
}
