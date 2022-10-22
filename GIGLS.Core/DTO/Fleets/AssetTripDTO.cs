using POST.Core.Enums;
using POST.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO.Fleets
{
    public class AssetTripDTO : BaseDomainDTO
    {
        public decimal TripAmount { get; set; }
        public FleetTripStatus Status { get; set; }
        public string DepartureCity { get; set; }
        public string DestinationCity { get; set; }
    }
}
