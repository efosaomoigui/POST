using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Account
{
    public class InvoiceReportDTO : BaseDomainDTO
    {
        public decimal Revenue { get; set; }
        public int ShipmentOrdered { get; set; }
        public int ShipmentDelivered { get; set; }

        public decimal IndividualRevenue { get; set; }
        public decimal CorporateRevenue { get; set; }
        public decimal EcommerceRevenue { get; set; }

        public int IndividualShipments { get; set; }
        public int CorporateShipments { get; set; }
        public int EcommerceShipments { get; set; }

        public decimal AverageShipmentIndividual { get; set; }
        public decimal AverageShipmentECommerce { get; set; }
        public decimal AverageShipmentCorporate { get; set; }

        public int ECommerceHomeDeliveries { get; set; }
        public int ECommerceTerminalPickups { get; set; }

        public int HomeDeliveries { get; set; }
        public int TerminalPickups { get; set; }

        public List<object> Sales { get; set; }

        public decimal AvgOriginCostPerServiceCenter { get; set; }
        public decimal AvgDestCostPerServiceCenter { get; set; }

        public int CreatedShipments { get; set; }
        public int DepartedShipments { get; set; }

    }
}
