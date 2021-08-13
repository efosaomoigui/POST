using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Routes
{
    public class RouteDto
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }


        public int DepartureCenterId { get; set; }
        public int DestinationCenterId { get; set; }

        public string DepartureTerminalTitle { get; set; }
        public string DestinationTerminalTitle { get; set; }

        public bool IsMainRoute { get; set; }

        public int MainRouteId { get; set; }

    }
}
