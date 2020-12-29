using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Routes
{
    public class CreateRouteDto
    {
        public string RouteName { get; set; }
        [Required]
        public int DepartureCenterId { get; set; }
        [Required]
        public int DestinationCenterId { get; set; }
        public int RouteType { get; set; }
        public int? MainRouteId { get; set; }

        public decimal DispatchFee { get; set; }

        public decimal LoaderFee { get; set; }

        public decimal CaptainFee { get; set; }


    }
}
