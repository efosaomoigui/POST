using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Dashboard
{
    public class DashboardDTO : BaseDomainDTO
    {
        public int TotalShipmentDeliveries { get; set; }
        public int TotalShipmentOrders { get; set; }
        public int TotalCustomers { get; set; }
    }
}
