using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class EcommerceShipmentSummaryReportDTO : BaseDomainDTO
    {
        public string Waybill { get; set; }
        public int DepartureServiceCentreId { get; set; }
        public ServiceCentreDTO DepartureServiceCentre { get; set; }
        public int DestinationServiceCentreId { get; set; }
        public ServiceCentreDTO DestinationServiceCentre { get; set; }
        public CustomerDTO CustomerDetails { get; set; }
        public string CreationSource { get; set; }
        public string CurrentStatus { get; set; }
        public string ShipmentScanStatus { get; set; }
    }
}
