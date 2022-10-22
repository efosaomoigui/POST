using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Fleets
{
    public class FleetDisputeMessageDto
    {
        public string VehicleNumber { get; set; }
        public string DisputeMessage { get; set; }
        public string FleetOwnerId { get; set; }
    }
    
    public class FleetDisputeMessageMailDto
    {
        public string VehicleNumber { get; set; }
        public string DisputeMessage { get; set; }
        public string FleetManager { get; set; }
        public string FleetOwnerId { get; set; }
        public string FleetOwnerEmail { get; set; }
    }
}
