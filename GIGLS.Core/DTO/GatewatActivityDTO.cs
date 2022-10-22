using POST.Core.DTO.ServiceCentres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
{
    public class GatewatActivityDTO
    {
        public string ManifestNumber { get; set; }
        public string Waybill { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }

        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateTime { get; set; }
    }
}
