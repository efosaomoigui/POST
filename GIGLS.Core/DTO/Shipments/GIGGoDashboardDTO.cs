using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Shipments
{
    public class GIGGoDashboardDTO
    {
        public int GIGGoDashboardId { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal PartnerEarnings { get; set; }
        public int DeliveredShipments { get; set; }
        public int ShipmentRequests { get; set; }
        public int PickedupShipments { get; set; }
        public int CancelledShipments { get; set; }
        public int AcceptedShipments { get; set; }
        public int ExternalPartners { get; set; }
        public int InternalPartners { get; set; }
    }
}
