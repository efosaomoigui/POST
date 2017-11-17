using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Dashboard
{
    public class DashboardDTO
    {
        public int TotalShipmentDelivered { get; set; }
        public int TotalShipmentOrdered { get; set; }
        public int TotalCustomers { get; set; }
        public ServiceCentreDTO ServiceCentre { get; set; }
    }
}
