using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
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
        public StationDTO Station { get; set; }
        public List<ShipmentOrderDTO> MostRecentOrder { get; set; }
        public List<GraphDataDTO> GraphData { get; set; }
        public GraphDataDTO CurrentMonthGraphData { get; set; }
        public List<ShipmentDTO> ShipmentsOrderedByServiceCenter { get; set; }
    }
}
