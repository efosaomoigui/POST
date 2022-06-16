using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Fleets
{
    public class VehicleAnalyticsDto
    {
        public string VehicleCurrentLocation { get; set; }
        public string VehicleAssignedCaptain { get; set; }
        public int TotalNumberOfTrip { get; set; }
        public decimal TotalRevenueGenerated { get; set; }
        public decimal TotalExpenses { get; set; }
        public int VehicleAge { get; set; }
    }
}
