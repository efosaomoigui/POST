using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Fleets
{
    public class FleetJobCardDto
    {
        public int FleetJobCardId { get; set; }
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int FleetManagerId { get; set; }
    }

    public class OpenFleetJobCardDto
    {
        public string VehicleNumber { get; set; }
        public decimal Amount { get; set; }
        public string VehiclePartToFix { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string EnterprisePartnerId { get; set; }
    }
}
